using Editor.Component;
using Editor.Component.Events;

namespace Editor.Core.Components;

public class ConnectionJoint : EditorComponentBase
{
    private IEventBusSubscriber _eventBus = default!;
    
    
    public IEntity Connection1 { get; set; } = default!;
    public IEntity Connection2 { get; set; } = default!;


    protected override void OnInit()
    {
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<EntityDestroyed>(OnEntityDestroyed);
    }

    private void OnEntityDestroyed(EntityDestroyed e)
    {
        if (!Entity.Alive)
        {
            return;
        }
        
        if (e.Entity == Connection1 || e.Entity == Connection2)
        {
            Context.Destroy(Entity);
        }
    }
}