using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components.Behaviors;

public class DragOnMouseMoveBehavior : OnMouseMoveBehavior
{
    private EditorContext _context = default!;
    
    private Position _positionComponent = default!;
    private Hoverable _hoverableComponent = default!;
    private Selectable? _selectableComponent = default!;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        base.OnInit(context, entity);

        _context = context;
        
        _positionComponent = entity.GetRequiredComponent<Position>()!;
        _hoverableComponent = entity.GetRequiredComponent<Hoverable>()!;
        _selectableComponent = entity.GetComponent<Selectable>()?.Component;
    }
    
    protected override void OnMouseMove(MouseMove e)
    {
        if (_context.MouseLocked)
        {
            return;
        }
        
        if ((!_hoverableComponent.Hovered && _selectableComponent?.Selected != true) || e.Button != MouseButton.Left)
        {
            return;
        }
        
        var delta = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels) - e.PositionConverter.ScreenToWorldSpace(e.OldPositionPixels);
        _positionComponent.Value += delta;
    }
}