namespace Editor.Core.Components.Diagrams;

public abstract class Node : EditorComponentBase
{
    public virtual string? Label { get; set; }
    
    public virtual void OnConnected(BranchNode parent, Connection connection) { }
}