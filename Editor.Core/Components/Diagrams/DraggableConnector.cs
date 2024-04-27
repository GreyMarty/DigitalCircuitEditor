using Editor.Component;
using Editor.Core.Events;

namespace Editor.Core.Components.Diagrams;

public class DraggableConnector : EditorComponentBase 
{
    private Position _positionComponent = default!;
    
    public ComponentRef<Connection> Connection { get; set; } = default!;
    public ComponentRef<BranchNode> Parent { get; set; } = default!;
    
    
    protected override void OnInit()
    {
        _positionComponent = Entity.GetRequiredComponent<Position>()!;

        Parent.Component!.SetGhostActive(Connection.Component!.Type, false);
        
        Events.Subscribe<MouseMove>(Context_OnMouseMove);
        Events.Subscribe<MouseButtonUp>(Context_OnMouseButtonUp);
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<MouseMove>();
        Events.Unsubscribe<MouseButtonUp>();
    }

    private void Context_OnMouseMove(MouseMove e)
    {
        _positionComponent.Value = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels);
    }

    private void Context_OnMouseButtonUp(MouseButtonUp e)
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

            if (entity.GetComponent<Node>() is not { } node || node?.Component is OutputNode)
            {
                continue;
            }

            Parent.Component.Connect(Connection.Component.Type, node!);
            break;
        }
        
        Parent.Component!.SetGhostActive(Connection.Component!.Type, true);
        Context.Destroy(Entity);
    }
}