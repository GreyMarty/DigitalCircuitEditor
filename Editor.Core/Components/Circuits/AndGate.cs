using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class AndGate : LogicGate
{
    public override IEntity[] Ports { get; } = new IEntity[3];
    public IEntity InA { get => Ports[0]; set => Ports[0] = value; }
    public IEntity InB { get => Ports[1]; set => Ports[1] = value; }
    public IEntity Out { get => Ports[2]; set => Ports[2] = value; }
}