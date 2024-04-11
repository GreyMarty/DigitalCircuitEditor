namespace Editor.Core.Components.Diagrams;

public class ConstNode : Node 
{
    public bool Value { get; set; }
    public override string? Label 
    { 
        get => Value ? "1" : "0";
        set { }
    }
}