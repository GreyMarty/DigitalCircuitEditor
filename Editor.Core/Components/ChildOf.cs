using Editor.Component;
using Editor.Component.Events;

namespace Editor.Core.Components;

public class ChildOf : EditorComponentBase
{
    private IEventBusSubscriber _eventBus = default!;
    
    
    public bool DestroyWithParent { get; set; }
    public IEntity? Parent { get; set; }


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
        if (DestroyWithParent && e.Entity == Parent)
        {
            Context.Destroy(Entity);
        }
    }
}