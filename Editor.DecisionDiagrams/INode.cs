namespace Editor.DecisionDiagrams;

public interface INode
{
    public int Id { get; }
    public bool IsTerminal { get; }
}