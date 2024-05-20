namespace Editor.DecisionDiagrams.Extensions;

public static class DiagramReductionExtensions
{
    public static INode Reduce(this INode root)
    {
        while (ReduceI(root) | ReduceS(ref root, [])) { }

        return root;
    }
    
    private static bool ReduceI(INode root)
    {
        var result = false;
        
        foreach (var childA in root)
        {
            foreach (var childB in root)
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
                
                foreach (var parent in root.ParentsOf(childB))
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

    private static bool ReduceS(ref INode root, HashSet<int> reduced)
    {
        var result = false;
        
        if (root is not BranchNode branchNode || reduced.Contains(root.Id))
        {
            return false;
        }
        
        reduced.Add(root.Id);

        var trueNode = branchNode.True;
        var falseNode = branchNode.False;

        result |= ReduceS(ref trueNode, reduced);
        result |= ReduceS(ref falseNode, reduced);

        branchNode.True = trueNode;
        branchNode.False = falseNode;

        if (trueNode.IsIdenticalTo(falseNode))
        {
            root = trueNode;
            result = true;
        }
        
        return result;
    }
}