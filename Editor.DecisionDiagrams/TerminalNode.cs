namespace Editor.DecisionDiagrams;

public record TerminalNode(int Id, bool Value) : INode
{
    public bool IsTerminal => true;
}