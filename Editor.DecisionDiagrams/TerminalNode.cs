namespace Editor.DecisionDiagrams;

public class TerminalNode(bool value) : INode
{
    public bool IsTerminal => true;
    public bool Value { get; set; } = value;
}