using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Components;

public class DiagramNode<TConnectionType> : EditorComponentBase
    where TConnectionType : notnull
{
    private IEventBusSubscriber _eventBus = default!;
    
    public Dictionary<TConnectionType, IEntity> GhostConnections { get; } = [];
    public Dictionary<TConnectionType, IEntity> GhostNodes { get; } = [];

    public Dictionary<TConnectionType, IEntity> Connections { get; } = [];
    public Dictionary<TConnectionType, IEntity> Nodes { get; } = [];

    public virtual void OnConnected(DiagramNode<TConnectionType> node, Connection<TConnectionType> connection) { }
    
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
        foreach (var dict in (ReadOnlySpan<Dictionary<TConnectionType, IEntity>>)[ Connections, Nodes ])
        {
            var key = dict.First(x => x.Value == e.Entity).Key;
            if (!dict.Remove(key))
            {
                continue;
            }
            
            GhostNodes[key].GetRequiredComponent<GhostNode<TConnectionType>>().Component!.Active = true;
            GhostNodes[key].GetRequiredComponent<Renderer>().Component!.Visible = true;
            GhostConnections[key].GetRequiredComponent<Renderer>().Component!.Visible = true;
        }
    }
}