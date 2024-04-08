using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Behaviors;

public abstract class OnMouseButtonDownBehavior : EditorComponentBase
{
    private IEventBusSubscriber _eventBus = default!;

    public MouseButton Button { get; set; } = MouseButton.Left;
    
    
    protected override void OnInit()
    {
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseButtonDown>(e =>
        {
            if (e.Button == Button)
            {
                OnMouseButtonDown(e);
            }
        });
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseButtonDown>();
    }
    
    protected abstract void OnMouseButtonDown(MouseButtonDown e);
}