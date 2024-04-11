using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public abstract class OnMouseMoveBehavior : EditorComponentBase
{
    protected override void OnInit()
    {
        Events.Subscribe<MouseMove>(OnMouseMove);
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<MouseMove>();
    }
    
    protected abstract void OnMouseMove(MouseMove e);
}