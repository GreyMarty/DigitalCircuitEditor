namespace Editor.DecisionDiagrams.Circuits;

public class Input(int id, int inputId) : ICircuitElement
{
    public int Id { get; set; }
    public bool IsTerminal => true;
    public int InputId { get; set; } = inputId;
}