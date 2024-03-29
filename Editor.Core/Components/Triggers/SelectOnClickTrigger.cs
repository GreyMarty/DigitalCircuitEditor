using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Components.Triggers;

public class SelectOnClickTrigger : OnClickTrigger
{
    private ComponentRef<Selectable> _selectableComponent = default!;


    public override void Init(EditorWorld world, IEntity entity)
    {
        _selectableComponent = entity.GetRequiredComponent<Selectable>();
        
        base.Init(world, entity);
    }

    protected override void OnClick(MouseButtonDown e, bool hovered)
    {
        if (e.Button == MouseButton.Left)
        {
            _selectableComponent.Component!.Selected = hovered;
        }
    }
}