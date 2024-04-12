using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.Behaviors.Filters;

public class SelectedFilter : TriggerFilterBase<EditorContext>
{
    private Selectable _selectableComponent = default!;
    
    
    public override bool CanFire()
    {
        return _selectableComponent.Selected;
    }

    protected override void OnInit()
    {
        base.OnInit();

        _selectableComponent = Entity.GetRequiredComponent<Selectable>()!;
    }
}