using Editor.Component;

namespace Editor.Core.Behaviors.Triggers.Args;

public interface IEventArgs<out TEvent> : ITriggerArgs
{
    public TEvent Event { get; }
}