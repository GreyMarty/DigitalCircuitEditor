using Editor.Component;

namespace Editor.Core.Behaviors.Triggers.Args;

public record ComponentChangedTriggerArgs(ComponentBase Component, string? PropertyName) : IComponentChangedArgs;