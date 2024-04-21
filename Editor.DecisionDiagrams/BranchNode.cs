namespace Editor.DecisionDiagrams;

public class BranchNode(int variableId, INode trueNode, INode falseNode) : INode
{
    public bool IsTerminal => false;

    public int VariableId { get; set; }
    
    public INode True { get; set; } = trueNode;
    public INode False { get; set; } = falseNode;
}