namespace Editor.Component.Events;

public delegate void ComponentEventHandler(object sender, ComponentEventArgs e);

    
public class ComponentEventArgs(ComponentBase component) : EventArgs
{
    public ComponentBase Component { get; } = component;
}