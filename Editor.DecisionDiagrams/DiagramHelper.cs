namespace Editor.DecisionDiagrams;

public static class DiagramHelper
{
    public static IEnumerable<INode> Enumerate(this INode root)
    {
        yield return root;

        if (root.IsTerminal)
        {
            yield break;
        }

        var branchRoot = (BranchNode)root;
        
        foreach (var node in Enumerate(branchRoot.True))
        {
            yield return node;
        }
        
        foreach (var node in Enumerate(branchRoot.False))
        {
            yield return node;
        }
    }

    public static int Depth(this INode node)
    {
        if (node.IsTerminal)
        {
            return 1;
        }

        var branchNode = (BranchNode)node;
        return Math.Max(Depth(branchNode.True), Depth(branchNode.False)) + 1;
    }
}