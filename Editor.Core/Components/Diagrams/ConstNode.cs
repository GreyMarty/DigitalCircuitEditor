namespace Editor.Core.Components.Diagrams;

public interface IConstNode
{
    public bool Value { get; set; }
}

public class ConstNode<TConnectionType> : Node<TConnectionType>, IConstNode 
    where TConnectionType : notnull
{
    public bool Value { get; set; }
    public override string? Label 
    { 
        get => Value ? "1" : "0";
        set { }
    }
}