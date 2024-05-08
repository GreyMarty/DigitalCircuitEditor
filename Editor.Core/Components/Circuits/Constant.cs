using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class Constant : LogicGate
{
    public bool Value { get; set; }
    public override IEntity[] Ports { get; } = new IEntity[1];
    public IEntity Out { get => Ports[0]; set => Ports[0] = value; }
}