namespace Editor.Core.Components.BinaryDiagrams;

public class BinaryDiagramConnection : Connection<BinaryDiagramConnectionType>
{
    public override string? Label
    {
        get => Type != BinaryDiagramConnectionType.Direct ? Type.ToString() : null;
        set { }
    }
}