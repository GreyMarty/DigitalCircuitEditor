using Editor.Component;
using Editor.Core.Input;

namespace Editor.Core.Behaviors.Triggers.Args;

public interface IMouseButtonArgs : ITriggerArgs
{
    public MouseButton Button { get; }
}