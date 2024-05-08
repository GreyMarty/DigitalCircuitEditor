using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class NotGate : LogicGate
{
    public override IEntity[] Ports { get; } = new IEntity[2];
    public IEntity In { get => Ports[0]; set => Ports[0] = value; }
    public IEntity Out { get => Ports[1]; set => Ports[1] = value; }
}