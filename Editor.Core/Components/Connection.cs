using Editor.Component;
using Editor.Component.Events;
using TinyMessenger;

namespace Editor.Core.Components;

public class Connection : EditorComponentBase
{
    private EditorContext _context = default!;
    private IEntity _entity = default!;

    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken _entityDestroyedToken = default!;
    
        
    public IEntity? Target { get; set; }
    public virtual string? Label { get; set; }


    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _context = context;
        _entity = entity;

        _eventBus = _context.EventBus;
        _entityDestroyedToken = _eventBus.Subscribe<EntityDestroyed>(OnEntityDestroyed);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<EntityDestroyed>(_entityDestroyedToken);
    }

    private void OnEntityDestroyed(EntityDestroyed e)
    {
        if (e.Entity == Target)
        {
            _context.Destroy(_entity);
        }
    }
}
