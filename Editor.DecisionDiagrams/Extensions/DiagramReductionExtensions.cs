namespace Editor.DecisionDiagrams.Extensions;

public static class DiagramReductionExtensions
{
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

                if (!childA.IsIdenticalTo(childB))
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

        if (trueNode.IsIdenticalTo(falseNode))
        {
            node = trueNode;
            result = true;
        }
        
        return result;
    }
}