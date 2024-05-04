namespace Editor.DecisionDiagrams.Circuits.Gates;

public class MuxGate(int id, ICircuitElement inputLow, ICircuitElement inputHigh, ICircuitElement inputSelector) : ILogicGate
{
    public int Id { get; } = id;
    public bool IsTerminal => false;
    public ICircuitElement[] Inputs { get; } = [inputLow, inputHigh, inputSelector];
}