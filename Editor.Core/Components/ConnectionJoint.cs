using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Adapters;

namespace Editor.Core.Components;

public class ConnectionJoint : EditorComponentBase
{
    public IEntity Connection1 { get; set; } = default!;
    public IEntity Connection2 { get; set; } = default!;


    protected override void OnInit()
    {
        Events.Subscribe<EntityDestroying>(Context_OnEntityDestroying);
        Events.Subscribe<EntityDestroyed>(Context_OnEntityDestroyed);
    }

    private void Context_OnEntityDestroying(EntityDestroying e)
    {
        if (e.Entity != Entity)
        {
            return;
        }
        
        var connection1Component = Connection1.GetRequiredComponent<Connection>().Component!;
        connection1Component.Target = Connection2.GetRequiredComponent<Connection>().Component!.Target;
        
        if (Connection1.GetComponent<ConnectionToLineShapeAdapter>()?.Component is { } connectionAdapter)
        {
            connectionAdapter.IgnoreTargetShape = connection1Component.Target!.GetComponent<ConnectionJoint>() is not null;
        }
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