namespace Editor.DecisionDiagrams.Circuits.Gates;

public class AndGate(int id, ICircuitElement input1, ICircuitElement input2) : ILogicGate
{
    public int Id { get; } = id;
    public bool IsTerminal => false;
    public ICircuitElement[] Inputs { get; } = [input1, input2];
}