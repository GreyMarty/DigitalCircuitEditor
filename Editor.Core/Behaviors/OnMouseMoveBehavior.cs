using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public abstract class OnMouseMoveBehavior : EditorComponentBase
{
    private IEventBusSubscriber _eventBus = default!;
    
    
    protected override void OnInit()
    {
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseMove>(OnMouseMove);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseMove>();
    }
    
    protected abstract void OnMouseMove(MouseMove e);
}