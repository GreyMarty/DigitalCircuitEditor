using Editor.Component;
using Editor.Core.Events;
using TinyMessenger;

namespace Editor.Core.Components.Triggers;

public abstract class OnClickTrigger : EditorComponentBase
{
    private ComponentRef<Hoverable> _hoverableComponent = default!;
    
    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken _mouseDownToken = default!;
    
    
    public override void Init(EditorWorld world, IEntity entity)
    {
        _hoverableComponent = entity.GetRequiredComponent<Hoverable>();

        _eventBus = world.EventBus;
        _mouseDownToken = _eventBus.Subscribe<MouseButtonDown>(World_OnMouseButtonDown);
    }

    public override void Dispose()
    {
        _eventBus.Unsubscribe<MouseButtonDown>(_mouseDownToken);
        
        base.Dispose();
    }

    protected abstract void OnClick(MouseButtonDown e, bool hovered);
    
    private void World_OnMouseButtonDown(MouseButtonDown e)
    {
        OnClick(e, _hoverableComponent.Component?.Hovered == true);
    }
}