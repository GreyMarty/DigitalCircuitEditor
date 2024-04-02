using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components.Behaviors;

public class DragOnMouseMoveBehavior : OnMouseMoveBehavior
{
    private Position _positionComponent = default!;
    private Hoverable _hoverableComponent = default!;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        base.OnInit(context, entity);
        
        _positionComponent = entity.GetRequiredComponent<Position>()!;
        _hoverableComponent = entity.GetRequiredComponent<Hoverable>()!;
    }
    
    protected override void OnMouseMove(MouseMove e)
    {
        if (!_hoverableComponent.Hovered || e.Button != MouseButton.Left)
        {
            return;
        }
        
        var delta = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels) - e.PositionConverter.ScreenToWorldSpace(e.OldPositionPixels);
        _positionComponent.Value += delta;
    }
}