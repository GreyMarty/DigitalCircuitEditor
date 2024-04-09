using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Events;

namespace Editor.Core.Components;

public class DraggableConnector<TConnectionType> : EditorComponentBase 
    where TConnectionType : notnull
{
    private Position _positionComponent = default!;
    
    private IEventBusSubscriber _eventBus = default!;
    
    public ComponentRef<Connection<TConnectionType>> Connection { get; set; } = default!;
    public ComponentRef<DiagramNode<TConnectionType>> Parent { get; set; } = default!;
    
    
    protected override void OnInit()
    {
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
        
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseMove>(OnMouseMove);
        _eventBus.Subscribe<MouseButtonUp>(OnMouseButtonUp);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseMove>();
        _eventBus.Unsubscribe<MouseButtonUp>();
    }

    private void OnMouseMove(MouseMove e)
    {
        _positionComponent.Value = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels);
    }

    private void OnMouseButtonUp(MouseButtonUp e)
    {
        if (Parent.Component is null || Connection.Component is null)
        {
            Context.Destroy(Entity);
            return;
        }
        
        foreach (var entity in Context.Entities)
        {
            if (entity.GetComponent<Hoverable>()?.Component?.Hovered != true)
            {
                continue;
            }

            if (entity.GetComponent<DiagramNode<TConnectionType>>() is not { } node)
            {
                continue;
            }

            Connection.Component.Target = entity;
            
            Parent.Component.Connections[Connection.Component.Type] = Connection.Component.Entity;
            Parent.Component.Nodes[Connection.Component.Type] = entity;
            
            node.Component?.OnConnected(Parent.Component, Connection.Component);
            break;
        }
        
        Context.Destroy(Entity);
    }
}