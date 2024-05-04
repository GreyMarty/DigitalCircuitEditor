namespace Editor.DecisionDiagrams.Circuits;

public class Constant(int id, bool value) : ICircuitElement
{
    public int Id { get; } = id;
    public bool IsTerminal => true;
    public bool Value { get; set; } = value;
}