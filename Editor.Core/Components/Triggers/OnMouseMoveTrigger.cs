using Editor.Component;
using Editor.Core.Events;
using TinyMessenger;

namespace Editor.Core.Components.Triggers;

public abstract class OnMouseMoveTrigger : ComponentBase<EditorWorld>
{
    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken _mouseMoveToken = default!;
    
    
    public override void Init(EditorWorld world, IEntity entity)
    {
        _eventBus = world.EventBus;
        _mouseMoveToken = _eventBus.Subscribe<MouseMove>(World_OnMouseMove);
    }

    public override void Dispose()
    {
        _eventBus.Unsubscribe<MouseButtonDown>(_mouseMoveToken);

        base.Dispose();
    }

    protected abstract void OnMouseMove(MouseMove e);
    
    private void World_OnMouseMove(MouseMove e)
    {
        OnMouseMove(e);
    }
}