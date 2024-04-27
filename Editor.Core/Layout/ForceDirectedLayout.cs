using System.Numerics;
using Editor.Core.Extensions;
using Editor.DecisionDiagrams;
using BranchNode = Editor.DecisionDiagrams.BranchNode;

namespace Editor.Core.Layout;

public class ForceDirectedLayout : ILayout
{
    private const float Eps = 0.001f;

    public int Iterations { get; set; } = 300;
    public float? ForceThreshold { get; set; }
    public float Stiffness { get; set; } = 5;
    public float PassiveStiffness { get; set; } = 1;
    public float NodeDistance { get; set; } = 5;
    public float PassiveNodeDistance { get; set; } = 10;
    public float Repulsion { get; set; } = 5;
    public float JointRepulsionModifier { get; set; } = 0.5f;
    public float Gravity { get; set; } = 1f;
    public int MaxJointsPerSegment { get; set; } = 1;
    public float JointAngularStiffness { get; set; } = 2f;
    public bool DoReduceJoints { get; set; } = true;
    public float Step { get; set; } = 0.1f;
    public float UpscaleFactor { get; set; } = 2f;
    
    
    public NodeLayoutInfo Arrange(BranchNode root)
    {
        SetUp(root, out var nodes, out var joints);

        var allNodes = nodes.Values
            .Concat(joints.Values.SelectMany(x => x))
            .ToList();

        for (var i = 0; i < Iterations; i++)
        {
            foreach (var node in allNodes)
            {
                var force = Vector2.Zero;
                
                foreach (var anotherNode in allNodes)
                {
                    if (ReferenceEquals(node, anotherNode))
                    {
                        continue;
                    }
                
                    force += CalculateRepulsion(node, anotherNode);
                    force += CalculatePassiveAttraction(node, anotherNode);
                }

                if (!node.IsJoint)
                {
                    force += Vector2.UnitY * Gravity;
                }
                
                node.Position += force * Step;
            }

            nodes[root.Id].Position -= Gravity * Step * Vector2.UnitY;
            
            ApplyAttraction(root, nodes, joints);
            
            foreach (var (direction, jointCollection) in joints)
            {
                var a = nodes[direction.Item1];
                var b = nodes[direction.Item2];
                
                foreach (var node in jointCollection)
                {
                    node.Position += CalculateJointStiffness(a, b, node) * Step;
                }
            }
        }

        var offset = nodes[root.Id].Position;
        
        foreach (var node in allNodes)
        {
            node.Position -= offset;
        }

        if (DoReduceJoints)
        {
            foreach (var (direction, jointCollection) in joints)
            {
                joints[direction] = ReduceJoints(nodes[direction.Item1], nodes[direction.Item2], jointCollection);
            }
        }
        
        return new NodeLayoutInfo(
            nodes.ToDictionary(x => x.Key, x => x.Value.Position * UpscaleFactor),
            joints.ToDictionary(x => x.Key, x => x.Value.Select(x => x.Position * UpscaleFactor))
        );
    }

    private void SetUp(BranchNode root, out Dictionary<int, ILayoutNode> nodes, out Dictionary<(int, int), IEnumerable<ILayoutNode>> joints)
    {
        var depth = root.Depth();

        nodes = new Dictionary<int, ILayoutNode>()
        {
            [root.Id] = new LayoutNode()
        };

        joints = new Dictionary<(int, int), IEnumerable<ILayoutNode>>();
        
        SetUp(root, nodes, joints, depth);
    }

    private void SetUp(INode root, Dictionary<int, ILayoutNode> nodes, Dictionary<(int, int), IEnumerable<ILayoutNode>> joints, int depth)
    {
        if (root.IsTerminal)
        {
            return;
        }

        var branchRoot = (BranchNode)root;

        var position = nodes[root.Id].Position;
        
        nodes[branchRoot.True.Id] = new LayoutNode
        {
            Position = position + new Vector2(-depth * NodeDistance, depth * NodeDistance)
        };

        nodes[branchRoot.False.Id] = new LayoutNode
        {
            Position = position + new Vector2(depth * NodeDistance, depth * NodeDistance)
        };

        joints[(root.Id, branchRoot.True.Id)] = Enumerable.Range(0, MaxJointsPerSegment)
            .Select(i => new LayoutNode(true)
            {
                Position = (position + nodes[branchRoot.True.Id].Position) / (MaxJointsPerSegment + 1) * (i + 1)
            })
            .ToArray();
        
        joints[(root.Id, branchRoot.False.Id)] = Enumerable.Range(0, MaxJointsPerSegment)
            .Select(i => new LayoutNode(true)
            {
                Position = (position + nodes[branchRoot.False.Id].Position) / (MaxJointsPerSegment + 1) * (i + 1)
            })
            .ToArray();
        
        SetUp(branchRoot.True, nodes, joints, depth - 1);
        SetUp(branchRoot.False, nodes, joints, depth - 1);
    }

    private void ApplyAttraction(INode root, Dictionary<int, ILayoutNode> nodes, Dictionary<(int, int), IEnumerable<ILayoutNode>> joints)
    {
        if (root.IsTerminal)
        {
            nodes[root.Id].Position += Gravity * Step * Vector2.UnitY;
            return;
        }
        
        var branchRoot = (BranchNode)root;

        ApplyAttraction(branchRoot.True, nodes, joints);
        ApplyAttraction(branchRoot.False, nodes, joints);

        var sign = -1;
        
        foreach (var child in new[] { branchRoot.True, branchRoot.False })
        {
            var source = nodes[branchRoot.Id];

            foreach (var target in joints[(branchRoot.Id, child.Id)].Append(nodes[child.Id]))
            {
                var force = CalculateAttraction(source, target, 2) * Step;
                
                source.Position += force;
                target.Position += sign * Gravity * Step * Vector2.UnitX - force;
            
                source = target;
            }

            sign *= -1;
        }
    }
    
    private Vector2 CalculateAttraction(ILayoutNode a, ILayoutNode b, float distanceModifier = 1)
    {
        EnsureNotTheSamePoint(a, b);

        var direction = (b.Position - a.Position) * distanceModifier;
        var distance = direction.Length();
        
        return Vector2.Normalize(direction) * Stiffness * MathF.Log(distance / NodeDistance);
    }

    private Vector2 CalculatePassiveAttraction(ILayoutNode a, ILayoutNode b)
    {
        EnsureNotTheSamePoint(a, b);

        var direction = b.Position - a.Position;
        var distance = direction.Length();
        
        return Vector2.Normalize(direction) * PassiveStiffness * MathF.Log(distance / PassiveNodeDistance) * CalculateForceModifier(a, b);
    }

    private Vector2 CalculateRepulsion(ILayoutNode a, ILayoutNode b)
    {
        EnsureNotTheSamePoint(a, b);

        var direction = a.Position - b.Position;
        var distance = direction.Length();

        return Vector2.Normalize(direction) * Repulsion / MathF.Pow(distance, 2) * CalculateForceModifier(a, b);
    }

    private Vector2 CalculateJointStiffness(ILayoutNode a, ILayoutNode b, ILayoutNode joint)
    {
        var ab = b.Position - a.Position;
        var abNormal = Vector2.Normalize(new Vector2(-ab.Y, ab.X));
        
        var delta = joint.Position - a.Position;
        var angle = MathF.Atan2(delta.Y, delta.X) - MathF.Atan2(ab.Y, ab.X);

        return -MathF.Sin(angle) * JointAngularStiffness * abNormal;
    }

    private IEnumerable<ILayoutNode> ReduceJoints(ILayoutNode a, ILayoutNode b, IEnumerable<ILayoutNode> joints)
    {
        var ab = b.Position - a.Position;
        var abNormal = Vector2.Normalize(new Vector2(-ab.Y, ab.X));

        return joints.Where(x =>
        {
            var delta = Vector2.Normalize(x.Position - a.Position);
            var distance = MathF.Abs(Vector2.Dot(delta, abNormal));
            
            return distance > 0.5f;
        })
        .ToList();
    }
    
    private void EnsureNotTheSamePoint(ILayoutNode a, ILayoutNode b)
    {
        if (a.Position.X == b.Position.X && a.Position.Y == b.Position.Y)
        {
            a.Position += -Vector2.UnitY * Eps;
        }
    }

    private float CalculateForceModifier(ILayoutNode a, ILayoutNode b)
    {
        var forceModifier = 1f;
        
        if (a.IsJoint)
        {
            forceModifier *= JointRepulsionModifier;
        }

        if (b.IsJoint)
        {
            forceModifier *= JointRepulsionModifier;
        }

        return forceModifier;
    }
}