using Editor.Component;

namespace Editor.Core.Components;

public class ChildOf : EditorComponentBase
{
    public IEntity? Parent { get; set; }
}