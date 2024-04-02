using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components.Behaviors;

public class SelectOnMouseButtonDownBehavior : OnMouseButtonDownBehavior
{
    private ComponentRef<Selectable> _selectableComponent = default!;


    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _selectableComponent = entity.GetRequiredComponent<Selectable>();
    }

    protected override void OnClick(MouseButtonDown e, bool hovered)
    {
        if (e.Button == MouseButton.Left)
        {
            _selectableComponent.Component!.Selected = hovered;
        }
    }
}