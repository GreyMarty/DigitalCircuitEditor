using Editor.Component;

namespace Editor.Core.Components.Circuits;

public class Input : EditorComponentBase
{
    public int VariableId { get; set; }
    public IEntity Out { get; set; } = default!;
}