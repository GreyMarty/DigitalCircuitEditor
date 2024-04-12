namespace Editor.Core.Behaviors.Triggers.Args;

public record EventTriggerArgs<TEvent>(TEvent Event) : IEventArgs<TEvent>;