using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components.Behaviors;

public class SelectOnMouseButtonDownBehavior : OnMouseButtonDownBehavior
{
    private Selectable _selectableComponent = default!;


    protected override void OnInit(EditorContext context, IEntity entity)
    {
        base.OnInit(context, entity);
        
        _selectableComponent = entity.GetRequiredComponent<Selectable>()!;
    }

    protected override void OnClick(MouseButtonDown e, bool hovered)
    {
        if (e.Button == MouseButton.Right)
        {
            _selectableComponent.Selected = hovered;
        }
    }
}