using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public abstract class OnMouseButtonUpBehavior : EditorComponentBase
{
    protected override void OnInit()
    {
        Events.Subscribe<MouseButtonUp>(OnMouseButtonUp);
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<MouseButtonUp>();
    }
    
    protected abstract void OnMouseButtonUp(MouseButtonUp e);
}