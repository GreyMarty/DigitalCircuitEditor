using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Prefabs;
using Editor.Core.Prefabs.Factories;
using TinyMessenger;

namespace Editor.Core.Components;

public class SelectionManager : EditorComponentBase
{
    private IEventBusSubscriber _eventBus = default!;


    public IEntityBuilderFactory SelectionAreaFactory { get; set; } = new SelectionAreaFactory();
    
    
    protected override void OnInit()
    {
        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseButtonDown>(OnMouseButtonDown);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseButtonDown>();
    }

    public void UnselectAll()
    {
        foreach (var entity in Context.Entities)
        {
            var selectableComponent = entity.GetComponent<Selectable>()?.Component;

            if (selectableComponent is null)
            {
                continue;
            }

            selectableComponent.Selected = false;
        }
    }
    
    private void OnMouseButtonDown(MouseButtonDown e)
    {
        if (e.Button != MouseButton.Left)
        {
            return;
        }

        Selectable? clickedOn = null;
        
        foreach (var entity in Context.Entities)
        {
            var hoverableComponent = entity.GetComponent<Hoverable>()?.Component;
            var selectableComponent = entity.GetComponent<Selectable>()?.Component;

            if (hoverableComponent?.Hovered == true && selectableComponent is null)
            {
                UnselectAll();
                return;
            }
            
            if (hoverableComponent?.Hovered != true || selectableComponent is null)
            {
                continue;
            }
            
            clickedOn = selectableComponent;
        }

        if (clickedOn?.Selected != true)
        {
            UnselectAll();
        }

        if (clickedOn is not null)
        {
            clickedOn.Selected = true;
            return;
        }

        var builder = SelectionAreaFactory.Create()
            .ConfigureComponent<Position>(x =>
            {
                x.Value = e.PositionConverter.ScreenToWorldSpace(e.PositionPixels);
            });
        
        Context.Instantiate(builder);
    }
}