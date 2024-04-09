using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Prefabs;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;

namespace Editor.Core.Behaviors;

public class ConnectToGhostNodeOnMouseButtonUp<TConnection, TConnectionType> : OnMouseButtonUpBehavior
    where TConnectionType : notnull
    where TConnection : Connection<TConnectionType>, new()
{
    private Position _positionComponent = default!;
    private Hoverable _hoverableComponent = default!;
    private Shape _shapeComponent = default!;


    public IEntityBuilderFactory ConnectionFactory { get; set; } = new ConnectionFactory<TConnection>();
    

    protected override void OnInit()
    {
        base.OnInit();

        _positionComponent = Entity.GetRequiredComponent<Position>()!;
        _hoverableComponent = Entity.GetRequiredComponent<Hoverable>()!;
        _shapeComponent = Entity.GetRequiredComponent<Shape>()!;
    }

    protected override void OnMouseButtonUp(MouseButtonUp e)
    {
        foreach (var entity in Context.Entities)
        {
            var positionComponent = entity.GetComponent<Position>()?.Component;
            var childOfComponent = entity.GetComponent<ChildOf>()?.Component;
            var ghostNodeComponent = entity.GetComponent<GhostNode<TConnectionType>>()?.Component;
            
            if (positionComponent is null || childOfComponent?.Parent is null || ghostNodeComponent?.Active != true)
            {
                continue;
            }
            
            var relativePosition = positionComponent.Value - _positionComponent.Value;
            if (!_shapeComponent.Contains(relativePosition))
            {
                continue;
            }

            var parentNode = childOfComponent.Parent.GetRequiredComponent<DiagramNode<TConnectionType>>().Component!;
            var connectionType = ghostNodeComponent.ConnectionType;
            
            var connection = Context.Instantiate(ConnectionFactory.Create()
                .ConfigureComponent<ChildOf>(x => x.Parent = childOfComponent.Parent)
                .ConfigureComponent<TConnection>(x =>
                {
                    x.Target = Entity;
                    x.Type = ghostNodeComponent.ConnectionType;
                })
            );

            parentNode.Connections[connectionType] = connection;
            parentNode.Nodes[connectionType] = Entity;
            
            Entity.GetRequiredComponent<DiagramNode<TConnectionType>>().Component?.OnConnected(parentNode, connection.GetRequiredComponent<Connection<TConnectionType>>()!);
            break;
        }
    }
}