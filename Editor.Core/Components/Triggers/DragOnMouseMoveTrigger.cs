using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components.Triggers;

public class DragOnMouseMoveTrigger : OnMouseMoveTrigger
{
    private ComponentRef<Position> _positionComponent = default!;
    private ComponentRef<Hoverable> _hoverableComponent = default!;
    
    
    public override void Init(EditorWorld world, IEntity entity)
    {
        _positionComponent = entity.GetRequiredComponent<Position>();
        _hoverableComponent = entity.GetRequiredComponent<Hoverable>();
        
        base.Init(world, entity);
    }
    
    protected override void OnMouseMove(MouseMove e)
    {
        if (_hoverableComponent.Component?.Hovered != true || e.Button != MouseButton.Left!)
        {
            return;
        }
        
        var delta = e.NewPosition - e.OldPosition;
        _positionComponent.Component!.Value += delta;
    }
}