namespace Editor.Core.Components.Diagrams.IfDiagrams;

public class IfDiagramConnection : Connection<IfDiagramConnectionType>
{
    public override string? Label
    {
        get => Type != IfDiagramConnectionType.Direct ? Type.ToString() : null;
        set { }
    }
}