using Editor.Component;

namespace Editor.Core.Components;

public class Connection : EditorComponentBase
{
    public Entity? Source { get; set; }
    public Entity? Target { get; set; }
}
