using Editor.Component;
using Editor.Component.Events;

namespace Editor.Core.Components;

public class ChildOf : EditorComponentBase
{
    public bool DestroyWithParent { get; set; }
    public IEntity? Parent { get; set; }


    protected override void OnInit()
    {
        Events.Subscribe<EntityDestroyed>(Context_OnEntityDestroyed, true);
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<EntityDestroyed>();
    }

    private void Context_OnEntityDestroyed(EntityDestroyed e)
    {
        if (DestroyWithParent && e.Entity == Parent)
        {
            Context.Destroy(Entity);
        }
    }
}