using Editor.Component;
using Editor.Component.Events;

namespace Editor.Core.Components;

public class DestroyWith : EditorComponentBase
{
    private IEventBusSubscriber _eventBus;
    
    
    public IEntity? Target { get; set; }


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