using System.Numerics;
using Editor.DecisionDiagrams;
using BranchNode = Editor.DecisionDiagrams.BranchNode;

namespace Editor.Core.Layout;

public class ForceDirectedLayout : ILayout
{
    private const float Eps = 0.001f;

    public int Iterations { get; set; } = 100;
    public float? ForceThreshold { get; set; }
    public float Stiffness { get; set; } = 2;
    public float PassiveStiffness { get; set; } = 2;
    public float NodeDistance { get; set; } = 3;
    public float PassiveNodeDistance { get; set; } = 5;
    public float Repulsion { get; set; } = 1;
    public float Step { get; set; } = 0.1f;
    
    
    public NodeLayoutInfo Arrange(BranchNode root)
    {
        SetUp(root, out var positions, out var joints);

        var nodePositions = positions
            .Where(x => x.Key != root)
            .Select(x => x.Value)
            .Concat(joints.Values.SelectMany(x => x))
            .ToList();

        for (var i = 0; i < Iterations; i++)
        {
            foreach (var nodePosition in nodePositions)
            {
                var force = Vector2.Zero;
                
                foreach (var anotherNodePosition in nodePositions.Append(positions[root]))
                {
                    if (ReferenceEquals(nodePosition, anotherNodePosition))
                    {
                        continue;
                    }
                
                    force += CalculateRepulsion(nodePosition, anotherNodePosition);
                    force += CalculatePassiveAttraction(nodePosition, anotherNodePosition);
                }

                nodePosition.Value += force * Step;
            }
        }
        
        ApplyAttraction(root, positions);

        return new NodeLayoutInfo(
            positions.ToDictionary(x => x.Key, x => x.Value.Value),
            joints.ToDictionary(x => x.Key, x => x.Value.Select(x => x.Value))
        );
    }

    private void SetUp(BranchNode root, out Dictionary<INode, Ref<Vector2>> positions, out Dictionary<(INode, INode), IEnumerable<Ref<Vector2>>> joints)
    {
        var depth = root.Depth();

        positions = new Dictionary<INode, Ref<Vector2>>
        {
            [root] = Vector2.Zero
        };
        joints = [];
        
        SetUp(root, positions, joints, depth);
    }

    private void SetUp(INode root, Dictionary<INode, Ref<Vector2>> positions, Dictionary<(INode, INode), IEnumerable<Ref<Vector2>>> joints, int depth)
    {
        if (root.IsTerminal)
        {
            return;
        }

        var branchRoot = (BranchNode)root;

        var position = positions[root];
        positions[branchRoot.True] = position + new Vector2(-depth * NodeDistance, depth * NodeDistance);
        positions[branchRoot.False] = position + new Vector2(depth * NodeDistance, depth * NodeDistance);

        joints[(root, branchRoot.True)] = [ (position.Value + positions[branchRoot.True]) / 2 ];
        joints[(root, branchRoot.False)] = [ (position.Value + positions[branchRoot.False]) / 2 ];

        SetUp(branchRoot.True, positions, joints, depth - 1);
        SetUp(branchRoot.False, positions, joints, depth - 1);
    }

    private void ApplyAttraction(INode root, Dictionary<INode, Ref<Vector2>> positions)
    {
        if (root.IsTerminal)
        {
            return;
        }
        
        var branchRoot = (BranchNode)root;

        ApplyAttraction(branchRoot.True, positions);
        ApplyAttraction(branchRoot.False, positions);

        var forceTrue = CalculateAttraction(positions[root], positions[branchRoot.True]) * Step;
        var forceFalse = CalculateAttraction(positions[root], positions[branchRoot.False]) * Step;

        positions[root] += (forceTrue + forceFalse);
        positions[branchRoot.True] += forceTrue;
        positions[branchRoot.False] += forceFalse;
    }
    
    private Vector2 CalculateAttraction(Vector2 a, Vector2 b)
    {
        EnsureNotTheSamePoint(ref a, ref b);

        var direction = b - a;
        var distance = direction.Length();
        
        return Vector2.Normalize(direction) * Stiffness * MathF.Log(distance / NodeDistance);
    }

    private Vector2 CalculatePassiveAttraction(Vector2 a, Vector2 b)
    {
        EnsureNotTheSamePoint(ref a, ref b);

        var direction = b - a;
        var distance = direction.Length();

        return Vector2.Normalize(direction) * PassiveStiffness * MathF.Log(distance / PassiveNodeDistance);
    }

    private Vector2 CalculateRepulsion(Vector2 a, Vector2 b)
    {
        EnsureNotTheSamePoint(ref a, ref b);

        var direction = a - b;
        var distance = direction.Length();

        return Vector2.Normalize(direction) * Repulsion / MathF.Pow(distance, 2);
    }

    private void EnsureNotTheSamePoint(ref Vector2 a, ref Vector2 b)
    {
        if (a.X == b.X && a.Y == b.Y)
        {
            a.Y += Eps;
        }
    }
}

internal class Ref<T>(T value)
{
    public T Value { get; set; } = value;

    public static implicit operator T(Ref<T> @ref) => @ref.Value;
    public static implicit operator Ref<T>(T value) => new Ref<T>(value);
}