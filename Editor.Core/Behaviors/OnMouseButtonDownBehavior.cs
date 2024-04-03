using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public abstract class OnMouseButtonDownBehavior : EditorComponentBase
{
    private IEventBusSubscriber _eventBus = default!;
    
    
    protected override void OnInit()
    {
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseButtonDown>(OnMouseButtonDown);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseButtonDown>();
    }
    
    protected abstract void OnMouseButtonDown(MouseButtonDown e);
}