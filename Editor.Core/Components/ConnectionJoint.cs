using Editor.Component;
using Editor.Component.Events;

namespace Editor.Core.Components;

public class ConnectionJoint : EditorComponentBase
{
    public IEntity Connection1 { get; set; } = default!;
    public IEntity Connection2 { get; set; } = default!;


    protected override void OnInit()
    {
        Events.Subscribe<EntityDestroyed>(Context_OnEntityDestroyed);
    }

    private void Context_OnEntityDestroyed(EntityDestroyed e)
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