using Editor.DecisionDiagrams.Operations;

namespace Editor.DecisionDiagrams.Extensions;

public static class DiagramOperationsExtensions
{
    public static INode Apply(this IOperation operation, params INode[] nodes)
    {
        var id = 0;

        return operation switch
        {
            IBinaryOperation binary => binary.Apply(nodes[0], nodes[1], ref id, []),
            IUnaryOperation unary => unary.Apply(nodes[0], ref id, []),
            _ => throw new InvalidOperationException()
        };
    }

    private static INode Apply(this IUnaryOperation operation, INode a, ref int id, Dictionary<int, INode> cache)
    {
        if (cache.TryGetValue(a.Id, out var node))
        {
            return node;
        }

        switch (a)
        {
            case TerminalNode ta:
                node = new TerminalNode(
                    id++,
                    operation.Compute(ta.Value)
                );
                cache[a.Id] = node;
                return node;
            
            case BranchNode ba:
                node = new BranchNode(
                    id++,
                    ba.VariableId,
                    operation.Apply(ba.True, ref id, cache),
                    operation.Apply(ba.False, ref id, cache)
                );
                cache[a.Id] = node;
                return node;
            
            default:
                throw new InvalidOperationException();
        }
    }
    
    private static INode Apply(this IBinaryOperation operation, INode a, INode b, ref int id, Dictionary<(int, int), INode> cache)
    {
        if (cache.TryGetValue((a.Id, b.Id), out var node))
        {
            return node;
        }

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
        
        var ba = a as BranchNode;
        var bb = b as BranchNode;
        
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