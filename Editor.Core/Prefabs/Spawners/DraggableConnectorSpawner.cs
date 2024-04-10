using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Prefabs.Factories;

namespace Editor.Core.Prefabs.Spawners;

public class DraggableConnectorSpawner<TConnection, TConnectionType> : Spawner 
    where TConnectionType : notnull 
    where TConnection : Connection<TConnectionType>, new()
{
    private ChildOf _childOfComponent = default!;
    private GhostNode<TConnectionType> _ghostNode = default!;


    public IEntityBuilderFactory ConnectorFactory = new DraggableConnectorFactory<TConnectionType>();
    public IEntityBuilderFactory ConnectionFactory = new ConnectionFactory<TConnection>();


    protected override void OnInit()
    {
        base.OnInit();
        
        _childOfComponent = Entity.GetRequiredComponent<ChildOf>()!;
        _ghostNode = Entity.GetRequiredComponent<GhostNode<TConnectionType>>()!;
    }

    protected override void OnSpawn(EditorContext context)
    {
        if (_childOfComponent.Parent is null)
        {
            return;
        }

        var parentNode = _childOfComponent.Parent.GetRequiredComponent<BranchNode<TConnectionType>>();
        
        var connection = Context.Instantiate(ConnectionFactory.Create()
            .ConfigureComponent<ChildOf>(x =>
            {
                x.Parent = _childOfComponent.Parent;
            })
            .ConfigureComponent<TConnection>(x =>
            {
                x.Target = _childOfComponent.Parent;
                x.Type = _ghostNode.ConnectionType;
            })
        );
        
        parentNode.Component!.Connections[_ghostNode.ConnectionType] = connection;

        var connectionComponent = connection.GetRequiredComponent<Connection<TConnectionType>>();
        
        var connector = context.Instantiate(ConnectorFactory.Create()
            .ConfigureComponent<Position>(x =>
            {
                x.Value = Position;
            })
            .ConfigureComponent<DraggableConnector<TConnectionType>>(x =>
            {
                x.Parent = parentNode;
                x.Connection = connectionComponent;
            })
        );
        
        connectionComponent.Component!.Target = connector;
    }
}