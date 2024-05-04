namespace Editor.DecisionDiagrams.Circuits.Gates;

public interface ILogicGate : ICircuitElement
{
    public ICircuitElement[] Inputs { get; }
}