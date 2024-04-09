using System.Collections.Specialized;
using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Collections;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Components;

public class DiagramNode<TConnectionType> : EditorComponentBase
    where TConnectionType : notnull
{
    private IEventBusSubscriber _eventBus = default!;
    
    public Dictionary<TConnectionType, IEntity> GhostConnections { get; } = [];
    public Dictionary<TConnectionType, IEntity> GhostNodes { get; } = [];

    public ObservableDictionary<TConnectionType, IEntity> Connections { get; } = [];
    public ObservableDictionary<TConnectionType, IEntity> Nodes { get; } = [];

    public virtual void OnConnected(DiagramNode<TConnectionType> node, Connection<TConnectionType> connection) { }
    
    protected override void OnInit()
    {
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<EntityDestroyed>(OnEntityDestroyed);
        
        Connections.CollectionChanged += Nodes_OnCollectionChanged;
        Nodes.CollectionChanged += Nodes_OnCollectionChanged;
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<EntityDestroyed>();
        
        Connections.CollectionChanged -= Nodes_OnCollectionChanged;
        Nodes.CollectionChanged -= Nodes_OnCollectionChanged;
    }

    private void OnEntityDestroyed(EntityDestroyed e)
    {
        foreach (var dict in (ReadOnlySpan<ObservableDictionary<TConnectionType, IEntity>>)[ Connections, Nodes ])
        {
            var key = dict.First(x => x.Value == e.Entity).Key;
            Connections.Remove(key);
            Nodes.Remove(key);
            break;
        }
    }
    
    private void Nodes_OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
            {
                foreach (var item in e.NewItems)
                {
                    var (key, value) = (KeyValuePair<TConnectionType, IEntity>)item;
                
                    GhostNodes[key].GetRequiredComponent<GhostNode<TConnectionType>>().Component!.Active = false;
                    GhostNodes[key].GetRequiredComponent<Renderer>().Component!.Visible = false;
                    GhostConnections[key].GetRequiredComponent<Renderer>().Component!.Visible = false;
                }

                break;
            }
            case NotifyCollectionChangedAction.Remove:
            {
                foreach (var item in e.OldItems)
                {
                    var (key, value) = (KeyValuePair<TConnectionType, IEntity>)item;
                
                    GhostNodes[key].GetRequiredComponent<GhostNode<TConnectionType>>().Component!.Active = true;
                    GhostNodes[key].GetRequiredComponent<Renderer>().Component!.Visible = true;
                    GhostConnections[key].GetRequiredComponent<Renderer>().Component!.Visible = true;
                }

                break;
            }
        }
    }
}