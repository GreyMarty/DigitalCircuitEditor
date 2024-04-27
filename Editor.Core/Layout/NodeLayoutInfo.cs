using System.Collections.Frozen;
using System.Numerics;
using Editor.DecisionDiagrams;

namespace Editor.Core.Layout;

public record NodeLayoutInfo
{
    private readonly FrozenDictionary<int, Vector2> _positions;
    private readonly FrozenDictionary<(int, int), IEnumerable<Vector2>> _joints;
    
    
    internal NodeLayoutInfo(Dictionary<int, Vector2> positions, Dictionary<(int, int), IEnumerable<Vector2>> joints)
    {
        _positions = positions.ToFrozenDictionary();
        _joints = joints.ToFrozenDictionary();
    }
    
    
    public Vector2? Position(INode node)
    {
        return _positions.TryGetValue(node.Id, out var position)
            ? position
            : null;
    }

    public IEnumerable<Vector2> Joints(INode source, INode target)
    {
        return _joints.TryGetValue((source.Id, target.Id), out var positions) 
            ? positions 
            : Enumerable.Empty<Vector2>();
    }
}