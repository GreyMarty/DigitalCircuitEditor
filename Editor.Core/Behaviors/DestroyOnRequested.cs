using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public class DestroyOnRequested : EditorComponentBase
{
    private Selectable _selectableComponent = default!;
    
    
    protected override void OnInit()
    {
        _selectableComponent = Entity.GetRequiredComponent<Selectable>()!;

        Events.Subscribe<DestroyRequested>(Context_OnDestroyRequested, true);
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<DestroyRequested>();
    }

    private void Context_OnDestroyRequested(DestroyRequested e)
    {
        if (!_selectableComponent.Selected)
        {
            return;
        }
        
        Context.Destroy(Entity);
    }
}