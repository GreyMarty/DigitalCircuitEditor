using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public abstract class OnMouseButtonUpBehavior : EditorComponentBase
{
    private IEventBusSubscriber _eventBus = default!;
    
    
    protected override void OnInit()
    {
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseButtonUp>(OnMouseButtonUp);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseButtonUp>();
    }
    
    protected abstract void OnMouseButtonUp(MouseButtonUp e);
}