using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Behaviors;

public class DragOnMouseMove : OnMouseMoveBehavior
{
    private Position _positionComponent = default!;
    private Hoverable _hoverableComponent = default!;
    private Selectable? _selectableComponent;
    
    
    protected override void OnInit()
    {
        base.OnInit();
        
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
        _hoverableComponent = Entity.GetRequiredComponent<Hoverable>()!;
        _selectableComponent = Entity.GetComponent<Selectable>()?.Component;
    }

    protected override void OnMouseMove(MouseMove e)
    {
        if (Context.MouseLocked)
        {
            return;
        }
        
        var hoveredOrSelected = _hoverableComponent.Hovered || _selectableComponent?.Selected == true;
        
        if (!hoveredOrSelected || e.Button != MouseButton.Left)
        {
            return;
        }
        
        var delta = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels) - e.PositionConverter.ScreenToWorldSpace(e.OldPositionPixels);
        _positionComponent.Value += delta;
    }
}