using Editor.DecisionDiagrams.Operations;

namespace Editor.DecisionDiagrams;

public static class DiagramHelper
{
    public static IEnumerator<INode> GetEnumerator(this INode root)
    {
        yield return root;

        if (root.IsTerminal)
        {
            yield break;
        }

        var branchRoot = (BranchNode)root;
        
        foreach (var node in branchRoot.True)
        {
            yield return node;
        }
        
        foreach (var node in branchRoot.False)
        {
            yield return node;
        }
    }
    
    public static int Depth(this INode root)
    {
        if (root.IsTerminal)
        {
            return 1;
        }

        var branchNode = (BranchNode)root;
        return Math.Max(Depth(branchNode.True), Depth(branchNode.False)) + 1;
    }

    public static bool IsIdenticalTo(this INode a, INode b)
    {
        return (a, b) switch
        {
            (TerminalNode ta, TerminalNode tb) => ta.Value == tb.Value,
            (BranchNode ba, BranchNode bb) => ba.VariableId == bb.VariableId &&
                                              IsIdenticalTo(ba.True, bb.True) &&
                                              IsIdenticalTo(ba.False, bb.False),
            _ => false
        };
    }

    public static IEnumerable<BranchNode> ParentsOf(this INode root, INode node)
    {
        foreach (var child in root)
        {
            if (child is not BranchNode branchChild || (branchChild.True.Id != node.Id && branchChild.False.Id != node.Id))
            {
                continue;
            }

            yield return branchChild;
        }
    }
}