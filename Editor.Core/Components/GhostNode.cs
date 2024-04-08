namespace Editor.Core.Components;

public class GhostNode<TConnectionType> : EditorComponentBase
    where TConnectionType : notnull
{
    public TConnectionType ConnectionType { get; set; }
    public bool Active { get; set; }
}