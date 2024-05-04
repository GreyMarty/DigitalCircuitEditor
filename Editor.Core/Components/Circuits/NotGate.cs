using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class NotGate : EditorComponentBase
{
    public IEntity In { get; set; } = default!;
    public IEntity Out { get; set; } = default!;
}