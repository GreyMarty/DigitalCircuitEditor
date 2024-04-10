namespace Editor.Core.Components.Diagrams;

public abstract class Node<TConnectionType> : EditorComponentBase where TConnectionType : notnull
{
    public virtual string? Label { get; set; }
    
    public virtual void OnConnected(BranchNode<TConnectionType> parent, Connection<TConnectionType> connection) { }
}