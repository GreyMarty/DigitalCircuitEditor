using System.Collections.Frozen;
using System.Numerics;

namespace Editor.DecisionDiagrams.Layout;

public record LayoutInfo
{
    private readonly FrozenDictionary<int, Vector2> _positions;
    private readonly FrozenDictionary<(int, int), IEnumerable<Vector2>> _joints;
    
    
    internal LayoutInfo(Dictionary<int, Vector2> positions, Dictionary<(int, int), IEnumerable<Vector2>> joints)
    {
        _positions = positions.ToFrozenDictionary();
        _joints = joints.ToFrozenDictionary();
    }
    
    
    public Vector2? Position(int id)
    {
        return _positions.TryGetValue(id, out var position)
            ? position
            : null;
    }

    public IEnumerable<Vector2> Joints(int sourceId, int targetId)
    {
        return _joints.TryGetValue((sourceId, targetId), out var positions) 
            ? positions 
            : Enumerable.Empty<Vector2>();
    }
}