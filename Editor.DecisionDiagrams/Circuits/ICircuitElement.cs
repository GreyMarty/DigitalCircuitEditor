namespace Editor.DecisionDiagrams.Circuits;

public interface ICircuitElement
{
    public int Id { get; }
    public bool IsTerminal { get; }
}