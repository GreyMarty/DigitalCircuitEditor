﻿using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class Input : LogicGate
{
    public override IEntity[] Ports { get; } = new IEntity[1];
    public int VariableId { get; set; }
    public bool Inverted { get; set; } = false;
    public IEntity Out { get => Ports[0]; set => Ports[0] = value; }
}