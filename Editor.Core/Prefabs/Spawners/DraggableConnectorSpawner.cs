using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Prefabs.Factories;

namespace Editor.Core.Prefabs.Spawners;

public class DraggableConnectorSpawner : Spawner 
{
    private ChildOf _childOfComponent = default!;
    private GhostNode _ghostNode = default!;


    public IEntityBuilderFactory ConnectorFactory = new DraggableConnectorFactory();
    public IEntityBuilderFactory ConnectionFactory = new ConnectionFactory();


    protected override void OnInit()
    {
        base.OnInit();
        
        _childOfComponent = Entity.GetRequiredComponent<ChildOf>()!;
        _ghostNode = Entity.GetRequiredComponent<GhostNode>()!;
    }

    protected override IEnumerable<IEntity> OnSpawn(EditorContext context)
    {
        var result = new List<IEntity>();
        
        if (_childOfComponent.Parent is null)
        {
            return Enumerable.Empty<IEntity>();
        }

        var parentNode = _childOfComponent.Parent.GetRequiredComponent<BranchNode>();
        
        var connection = Context.Instantiate(ConnectionFactory.Create()
            .ConfigureComponent<ChildOf>(x =>
            {
                x.Parent = _childOfComponent.Parent;
            })
            .ConfigureComponent<Connection>(x =>
            {
                x.Target = _childOfComponent.Parent;
                x.Type = _ghostNode.ConnectionType;
            })
        );
        
        result.Add(connection);
        
        parentNode.Component!.Connections[_ghostNode.ConnectionType] = connection;

        var connectionComponent = connection.GetRequiredComponent<Connection>();
        
        var connector = context.Instantiate(ConnectorFactory.Create()
            .ConfigureComponent<Position>(x =>
            {
                x.Value = Position;
            })
            .ConfigureComponent<DraggableConnector>(x =>
            {
                x.Parent = parentNode;
                x.Connection = connectionComponent;
            })
        );
        
        result.Add(connector);
        
        connectionComponent.Component!.Target = connector;

        return result;
    }
}