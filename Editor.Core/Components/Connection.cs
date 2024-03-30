using Editor.Component;

namespace Editor.Core.Components;

public class Connection : EditorComponentBase
{
    public IEntity? Target { get; set; }
}
