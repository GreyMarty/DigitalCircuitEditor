using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class MuxGate : LogicGate
{
    public override IEntity[] Ports { get; } = new IEntity[4];
    public IEntity InS0 { get => Ports[0]; set => Ports[0] = value; }
    public IEntity InS1 { get => Ports[1]; set => Ports[1] = value; }
    public IEntity InS { get => Ports[2]; set => Ports[2] = value; }
    public IEntity Out { get => Ports[3]; set => Ports[3] = value; }
}