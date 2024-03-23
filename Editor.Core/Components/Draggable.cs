using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components;

public class Draggable : ComponentBase<EditorWorld>
{
    private Position _positionComponent = default!;
    private Hoverable _hoverableComponent = default!;
    
    
    public override void Init(EditorWorld world, Entity entity)
    {
        _positionComponent = entity.GetRequiredComponent<Position>();
        _hoverableComponent = entity.GetRequiredComponent<Hoverable>();

        world.EventBus.Subscribe<MouseMove>(OnMouseMove);
    }
    
    private void OnMouseMove(MouseMove e)
    {
        if (!_hoverableComponent.Hovered || e.Button != MouseButton.Left!)
        {
            return;
        }
        
        var delta = e.NewPosition - e.OldPosition;
        _positionComponent.Value += delta;
    }
}