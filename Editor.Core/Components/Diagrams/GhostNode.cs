namespace Editor.Core.Components.Diagrams;

public class GhostNode<TConnectionType> : EditorComponentBase
    where TConnectionType : notnull
{
    public TConnectionType ConnectionType { get; set; }
}