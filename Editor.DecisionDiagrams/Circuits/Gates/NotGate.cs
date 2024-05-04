namespace Editor.DecisionDiagrams.Circuits.Gates;

public class NotGate(int id, ICircuitElement input) : ILogicGate
{
    public int Id { get; } = id;
    public bool IsTerminal => false;
    public ICircuitElement[] Inputs { get; } = [input];
}