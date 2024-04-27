using System.Collections.Specialized;
using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Collections;
using Editor.Core.Layout;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core.Components.Diagrams;

public abstract class BranchNode : Node
{
    private Dictionary<ConnectionType, Connection> _ghostConnections = [];
    private Dictionary<ConnectionType, GhostNode> _ghostNodes = [];
    private Dictionary<ConnectionType, Connection> _connections = [];
    private Dictionary<ConnectionType, Node> _nodes = [];
    
    private IEventBusSubscriber _eventBus = default!;


    public IEntityBuilderFactory ConnectionFactory { get; set; } = new ConnectionFactory();
    public IEntityBuilderFactory GhostConnectionFactory { get; set; } = new GhostConnectionFactory();
    
    public IReadOnlyDictionary<ConnectionType, Connection> GhostConnections => _ghostConnections;
    public IReadOnlyDictionary<ConnectionType, GhostNode> GhostNodes => _ghostNodes;

    public IReadOnlyDictionary<ConnectionType, Connection> Connections => _connections;
    public IReadOnlyDictionary<ConnectionType, Node> Nodes => _nodes;


    public virtual void AddGhost(GhostNode node)
    {
        var connectionType = node.ConnectionType;
        _ghostNodes[connectionType] = node;

        if (!_ghostConnections.TryGetValue(connectionType, out var connection))
        {
            var connectionEntity = Context.Instantiate(GhostConnectionFactory.Create()
                .ConfigureComponent<ChildOf>(x => x.Parent = Entity)
                .ConfigureComponent<Connection>(x =>
                {
                    x.Type = connectionType;
                    x.Target = node.Entity;
                })
            );
            connection = connectionEntity.GetRequiredComponent<Connection>();
            _ghostConnections[connectionType] = connection!;
        }
        
        SetGhostActive(connectionType, true);
    }

    public virtual void SetGhostActive(ConnectionType connectionType, bool active)
    {
        active &= !(_nodes.ContainsKey(connectionType) || _connections.ContainsKey(connectionType));
        
        _ghostNodes[connectionType].Entity.Active = active;
        _ghostNodes[connectionType].Entity.GetRequiredComponent<Renderer>().Component!.Visible = active;
            
        _ghostConnections[connectionType].Entity.Active = active;
        _ghostConnections[connectionType].Entity.GetRequiredComponent<Renderer>().Component!.Visible = active;
    }
    
    public virtual Connection? Connect(ConnectionType connectionType, Node? node)
    {
        if (node is null)
        {
            _nodes.Remove(connectionType);

            if (_connections.TryGetValue(connectionType, out var connectionToDestroy))
            {
                Context.Destroy(connectionToDestroy.Entity);
                _connections.Remove(connectionType);
            }

            SetGhostActive(connectionType, true);
            return null;
        }
        
        _nodes[connectionType] = node;

        if (!_connections.TryGetValue(connectionType, out var connection))
        {
            var connectionEntity = Context.Instantiate(ConnectionFactory.Create()
                .ConfigureComponent<ChildOf>(x => x.Parent = Entity)
                .ConfigureComponent<Connection>(x =>
                {
                    x.Type = connectionType;
                    x.Target = node.Entity;
                })
            );
            connection = connectionEntity.GetRequiredComponent<Connection>();
            _connections[connectionType] = connection!;
        }
        
        SetGhostActive(connectionType, false);
        node.OnConnected(this, connection!);
        
        return connection;
    }
    
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
        var nodePair = _nodes.FirstOrDefault(x => x.Value.Entity == e.Entity);
        var connectionPair = nodePair.Equals(default(KeyValuePair<ConnectionType, Node>))
            ? _connections.FirstOrDefault(x => x.Value.Entity == e.Entity)
            : default;

        var connectionType = default(ConnectionType);
        
        if (!nodePair.Equals(default(KeyValuePair<ConnectionType, Node>)))
        {
            connectionType = nodePair.Key;
        }
        else if (!connectionPair.Equals(default(KeyValuePair<ConnectionType, Connection>)))
        {
            connectionType = connectionPair.Key;
        }
        else
        {
            return;
        }

        Connect(connectionType, null);
    }
}