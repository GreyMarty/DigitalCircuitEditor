using Editor.Component;
using Editor.Component.Events;

namespace Editor.Core.Components.Diagrams;

public class Connection : EditorComponentBase
{
    public IEntity? Target { get; set; }
    public ConnectionType Type { get; set; }
    
    public virtual string? Label
    {
        get => Type != ConnectionType.Direct ? Type.ToString() : null;
        set { }
    }


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
