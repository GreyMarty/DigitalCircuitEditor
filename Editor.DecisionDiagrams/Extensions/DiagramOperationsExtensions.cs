using Editor.DecisionDiagrams.Operations;

namespace Editor.DecisionDiagrams.Extensions;

public static class DiagramOperationsExtensions
{
    public static INode Apply(this IBooleanOperation operation, INode a, INode b)
    {
        var id = 0;
        return operation.Apply(a, b, ref id, []);
    }

    private static INode Apply(this IBooleanOperation operation, INode a, INode b, ref int id, Dictionary<(int, int), INode> cache)
    {
        if (cache.TryGetValue((a.Id, b.Id), out var node))
        {
            return node;
        }

        var ba = a as BranchNode;
        var bb = b as BranchNode;
        
        // Both terminal
        if (a is TerminalNode ta && b is TerminalNode tb)
        {
            node = new TerminalNode(
                id++,
                operation.Compute(ta.Value, tb.Value)
            );
            cache[(a.Id, b.Id)] = node;
            return node;
        }
        
        // Both non-terminal, var(a) == var(b)
        if (ba is not null && bb is not null && ba.VariableId == bb.VariableId)
        {
            node = new BranchNode(
                id++,
                ba.VariableId,
                operation.Apply(ba.True, bb.True, ref id, cache),
                operation.Apply(ba.False, bb.False, ref id, cache)
            );
            cache[(a.Id, b.Id)] = node;
            return node;
        }

        // b is terminal
        // Both non-terminal, var(a) < var(b).
        if (ba is not null && (bb is null || ba.VariableId < bb.VariableId))
        {
            node = new BranchNode(
                id++,
                ba.VariableId,
                operation.Apply(ba.True, b, ref id, cache),
                operation.Apply(ba.False, b, ref id, cache)
            );
            cache[(a.Id, b.Id)] = node;
            return node;
        }
        
        // a is terminal
        // Both non-terminal, var(b) < var(a).
        if (bb is not null && (ba is null || bb.VariableId < ba.VariableId))
        {
            node = new BranchNode(
                id++,
                bb.VariableId,
                operation.Apply(a, bb.True, ref id, cache),
                operation.Apply(a, bb.False, ref id, cache)
            );
            cache[(a.Id, b.Id)] = node;
            return node;
        }

        throw new InvalidOperationException();
    }
}