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

    public static bool AreIdentical(INode a, INode b)
    {
        return (a, b) switch
        {
            (TerminalNode ta, TerminalNode tb) => ta.Value == tb.Value,
            (BranchNode ba, BranchNode bb) => ba.VariableId == bb.VariableId &&
                                              AreIdentical(ba.True, bb.True) &&
                                              AreIdentical(ba.False, bb.False),
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
    
    public static INode Reduce(this INode node)
    {
        while (ReduceI(node) | ReduceS(ref node, [])) { }

        return node;
    }

    private static bool ReduceI(INode node)
    {
        var result = false;
        
        foreach (var childA in node)
        {
            foreach (var childB in node)
            {
                if (childA.Id == childB.Id)
                {
                    continue;
                }

                if (!AreIdentical(childA, childB))
                {
                    continue;
                }

                result = true;
                
                foreach (var parent in node.ParentsOf(childB))
                {
                    if (parent.True.Id == childB.Id)
                    {
                        parent.True = childA;
                    }
                    else
                    {
                        parent.False = childA;
                    }
                }
            }
        }

        return result;
    }

    private static bool ReduceS(ref INode node, HashSet<int> reduced)
    {
        var result = false;
        
        if (node is not BranchNode branchNode || reduced.Contains(node.Id))
        {
            return false;
        }
        
        reduced.Add(node.Id);

        var trueNode = branchNode.True;
        var falseNode = branchNode.False;

        result |= ReduceS(ref trueNode, reduced);
        result |= ReduceS(ref falseNode, reduced);

        branchNode.True = trueNode;
        branchNode.False = falseNode;

        if (AreIdentical(trueNode, falseNode))
        {
            node = trueNode;
            result = true;
        }
        
        return result;
    }
}