namespace Editor.Core.Components.Diagrams.BinaryDiagrams;

public class BinaryDiagramConnection : Connection<BinaryDiagramConnectionType>
{
    public override string? Label
    {
        get => Type != BinaryDiagramConnectionType.Direct ? Type.ToString() : null;
        set { }
    }
}