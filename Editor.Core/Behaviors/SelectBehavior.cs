using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.Behaviors;

public class SelectBehavior : BehaviorBase<EditorContext>
{
    private Selectable _selectableComponent = default!;
    

    protected override void OnInit()
    {
        base.OnInit();
        
        _selectableComponent = Entity.GetRequiredComponent<Selectable>()!;
    }

    protected override void Perform()
    {
        _selectableComponent.Selected = true;
    }
}

public class UnselectBehavior : BehaviorBase<EditorContext>
{
    private Selectable _selectableComponent = default!;
    

    protected override void OnInit()
    {
        base.OnInit();
        
        _selectableComponent = Entity.GetRequiredComponent<Selectable>()!;
    }

    protected override void Perform()
    {
        _selectableComponent.Selected = false;
    }
}