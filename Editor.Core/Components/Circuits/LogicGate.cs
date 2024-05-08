using Editor.Component;

namespace Editor.Core.Components.Circuits;

public abstract class LogicGate : EditorComponentBase
{
    public abstract IEntity[] Ports { get; }
}