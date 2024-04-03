using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Behaviors;

public class SelectOnMouseButtonDown : OnMouseButtonDownBehavior
{
    private Hoverable _hoverableComponent = default!;
    private Selectable _selectableComponent = default!;
    

    protected override void OnInit()
    {
        base.OnInit();
        
        _hoverableComponent = Entity.GetRequiredComponent<Hoverable>()!;
        _selectableComponent = Entity.GetRequiredComponent<Selectable>()!;
    }

    protected override void OnMouseButtonDown(MouseButtonDown e)
    {
        if (e.Button == MouseButton.Right)
        {
            _selectableComponent.Selected = _hoverableComponent.Hovered;
        }
    }
}