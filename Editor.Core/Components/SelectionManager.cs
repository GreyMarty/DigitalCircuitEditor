using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Prefabs;
using TinyMessenger;

namespace Editor.Core.Components;

public class SelectionManager : EditorComponentBase
{
    private EditorContext _context = default!;

    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken _mouseDownToken = default!;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _context = context;
        
        _eventBus = context.EventBus;
        _mouseDownToken = _eventBus.Subscribe<MouseButtonDown>(OnMouseButtonDown);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseButtonDown>(_mouseDownToken);
    }

    private void OnMouseButtonDown(MouseButtonDown e)
    {
        if (e.Button != MouseButton.Left)
        {
            return;
        }

        Selectable? clickedOn = null;
        
        foreach (var entity in _context.Entities)
        {
            var hoverableComponent = entity.GetComponent<Hoverable>()?.Component;
            var selectableComponent = entity.GetComponent<Selectable>()?.Component;
            
            if (hoverableComponent is null || selectableComponent is null || !hoverableComponent.Hovered)
            {
                continue;
            }
            
            clickedOn = selectableComponent;
        }

        if (clickedOn?.Selected != true)
        {
            foreach (var entity in _context.Entities)
            {
                var selectableComponent = entity.GetComponent<Selectable>()?.Component;

                if (selectableComponent is null)
                {
                    continue;
                }

                selectableComponent.Selected = false;
            }
        }

        if (clickedOn is not null)
        {
            clickedOn.Selected = true;
            return;
        }
        
        _context.Instantiate(SelectionAreaPrefab.CreateBuilder(e.PositionConverter.ScreenToWorldSpace(e.PositionPixels)));
    }
}