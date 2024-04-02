using Editor.Component;
using Editor.Component.Events;
using TinyMessenger;

namespace Editor.Core.Components;

public class ChildOf : EditorComponentBase
{
    private EditorContext _context = default!;
    private IEntity _entity = default!;
    
    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken _entityDestroyedToken = default!;
    
    
    public bool DestroyWithParent { get; set; }
    public IEntity? Parent { get; set; }


    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _context = context;
        _entity = entity;

        _eventBus = context.EventBus;
        _entityDestroyedToken = _eventBus.Subscribe<EntityDestroyed>(OnEntityDestroyed);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<EntityDestroyed>(_entityDestroyedToken);
    }

    private void OnEntityDestroyed(EntityDestroyed e)
    {
        if (DestroyWithParent && e.Entity == Parent)
        {
            _context.Destroy(_entity);
        }
    }
}