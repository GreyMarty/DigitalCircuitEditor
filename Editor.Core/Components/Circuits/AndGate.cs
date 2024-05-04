using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class AndGate : EditorComponentBase
{
    public IEntity InA { get; set; } = default!;
    public IEntity InB { get; set; } = default!;
    public IEntity Out { get; set; } = default!;
}