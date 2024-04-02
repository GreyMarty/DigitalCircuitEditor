using Editor.Component;
using Editor.Core.Events;
using TinyMessenger;

namespace Editor.Core.Components.Behaviors;

public abstract class OnMouseButtonDownBehavior : EditorComponentBase
{
    private ComponentRef<Hoverable> _hoverableComponent = default!;
    
    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken _mouseDownToken = default!;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _hoverableComponent = entity.GetRequiredComponent<Hoverable>();

        _eventBus = context.EventBus;
        _mouseDownToken = _eventBus.Subscribe<MouseButtonDown>(World_OnMouseButtonDown);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseButtonDown>(_mouseDownToken);
    }

    protected abstract void OnClick(MouseButtonDown e, bool hovered);
    
    private void World_OnMouseButtonDown(MouseButtonDown e)
    {
        OnClick(e, _hoverableComponent.Component?.Hovered == true);
    }
}