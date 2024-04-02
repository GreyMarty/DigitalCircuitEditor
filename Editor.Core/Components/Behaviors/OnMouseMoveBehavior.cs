using Editor.Component;
using Editor.Core.Events;
using TinyMessenger;

namespace Editor.Core.Components.Behaviors;

public abstract class OnMouseMoveBehavior : EditorComponentBase
{
    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken _mouseMoveToken = default!;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _eventBus = context.EventBus;
        _mouseMoveToken = _eventBus.Subscribe<MouseMove>(World_OnMouseMove);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseButtonDown>(_mouseMoveToken);
    }

    protected abstract void OnMouseMove(MouseMove e);
    
    private void World_OnMouseMove(MouseMove e)
    {
        OnMouseMove(e);
    }
}