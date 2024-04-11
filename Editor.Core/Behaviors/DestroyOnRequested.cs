using Editor.Component.Events;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public class DestroyOnRequested : EditorComponentBase
{
    private Selectable _selectableComponent = default!;

    private IEventBusSubscriber _eventBus = default!;
    
    
    protected override void OnInit()
    {
        _selectableComponent = Entity.GetRequiredComponent<Selectable>()!;

        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<DestroyRequested>(OnDestroyRequested);
    }

    private void OnDestroyRequested(DestroyRequested e)
    {
        if (!_selectableComponent.Selected)
        {
            return;
        }
        
        Context.Destroy(Entity);
    }
}