using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.Behaviors.Filters;

public class HoveredFilter : TriggerFilterBase<EditorContext>
{
    private Hoverable _hoverableComponent = default!;
    
    
    public override bool CanFire()
    {
        return _hoverableComponent.Hovered;
    }

    protected override void OnInit()
    {
        base.OnInit();

        _hoverableComponent = Entity.GetRequiredComponent<Hoverable>()!;
    }
}