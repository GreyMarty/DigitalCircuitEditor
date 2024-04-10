namespace Editor.Core.Components.Diagrams;

public class OutputNode<TConnectionType> : BranchNode<TConnectionType> where TConnectionType : notnull
{
    public int OutputId { get; set; }

    public override string? Label
    {
        get => $"F{OutputId}";
        set { }
    }
}