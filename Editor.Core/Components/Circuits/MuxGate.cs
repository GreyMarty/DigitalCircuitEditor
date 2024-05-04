using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class MuxGate : EditorComponentBase
{
    public IEntity InS0 { get; set; } = default!;
    public IEntity InS1 { get; set; } = default!;
    public IEntity InS { get; set; } = default!;
    public IEntity Out { get; set; } = default!;
}