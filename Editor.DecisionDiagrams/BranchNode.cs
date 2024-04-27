namespace Editor.DecisionDiagrams;

public record BranchNode(int Id, int VariableId, INode True, INode False) : INode
{
    public bool IsTerminal => false;
}