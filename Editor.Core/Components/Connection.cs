using Editor.Component;
using Editor.Component.Events;

namespace Editor.Core.Components;

public class Connection : EditorComponentBase
{
    private IEventBusSubscriber _eventBus = default!;
    
        
    public IEntity? Target { get; set; }
    public virtual string? Label { get; set; }


    protected override void OnInit()
    {
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<EntityDestroyed>(OnEntityDestroyed);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<EntityDestroyed>();
    }

    private void OnEntityDestroyed(EntityDestroyed e)
    {
        if (e.Entity == Target)
        {
            Context.Destroy(Entity);
        }
    }
}

public class Connection<TConnectionType> : Connection
    where TConnectionType: notnull
{
    public TConnectionType Type { get; set; }
}
