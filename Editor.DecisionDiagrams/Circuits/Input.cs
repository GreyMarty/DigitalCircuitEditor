namespace Editor.DecisionDiagrams.Circuits;

public class Input(int id, int inputId, bool inverted = false) : ICircuitElement
{
    public int Id { get; set; } = id;
    public bool IsTerminal => true;
    public bool Inverted { get; set; } = inverted;
    public int InputId { get; set; } = inputId;
}