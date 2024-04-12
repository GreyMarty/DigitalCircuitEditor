using Editor.Component;

namespace Editor.Core.Behaviors.Triggers.Args;

public interface IComponentChangedArgs : ITriggerArgs
{
    public ComponentBase Component { get; }
    public string? PropertyName { get; }
}