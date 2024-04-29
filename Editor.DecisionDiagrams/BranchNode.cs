namespace Editor.DecisionDiagrams;

public class BranchNode(int id, int variableId, INode trueNode, INode falseNode) : INode
{
    public int Id { get; } = id;
    public bool IsTerminal => false;
    public int VariableId { get; } = variableId;
    public INode True { get; set; } = trueNode;
    public INode False { get; set; } = falseNode;
}