namespace Editor.Core.Components;

public class IfDiagramConnection : Connection
{
    public IfDiagramConnectionType Type { get; set; }
    
    public override string? Label
    {
        get => Type != IfDiagramConnectionType.Direct ? Type.ToString() : null;
        set { }
    }
}

public enum IfDiagramConnectionType
{
    Direct,
    If,
    True,
    False
}