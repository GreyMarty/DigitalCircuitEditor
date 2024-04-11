using Editor.Component;
using Editor.Component.Events;

namespace Editor.Core.Components;

public class DestroyWith : EditorComponentBase
{
    public IEntity? Target { get; set; }


    protected override void OnInit()
    {
        Events.Subscribe<EntityDestroyed>(Context_OnEntityDestroyed);
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<EntityDestroyed>();
    }

    private void Context_OnEntityDestroyed(EntityDestroyed e)
    {
        if (e.Entity == Target)
        {
            Context.Destroy(Entity);
        }
    } 
}