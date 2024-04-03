using System.Numerics;
using Editor.Component.Events;
using Editor.Core.Events;
using Editor.Core.Shapes;

namespace Editor.Core.Components;

public class SelectionArea : EditorComponentBase
{
    private Position _positionComponent = default!;
    private RectangleShape _shapeComponent = default!;

    private IEventBusSubscriber _eventBus = default!;


    protected override void OnInit()
    {
        _positionComponent = Entity.GetRequiredComponent<Position>().Component!;
        _shapeComponent = Entity.GetRequiredComponent<RectangleShape>().Component!;

        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseMove>(OnMouseMove);

        Context.MouseLocked = true;
    }

    protected override void OnDestroy()
    {
        Context.MouseLocked = false;

        _eventBus.Unsubscribe<MouseMove>();
        _eventBus.Unsubscribe<MouseButtonUp>();
    }

    private void OnMouseMove(MouseMove e)
    {
        var halfSize = new Vector2(_shapeComponent.Width / 2, _shapeComponent.Height / 2);
        var min = _positionComponent.Value - halfSize;
        var max = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels);

        _positionComponent.Value = (min + max) / 2;
        _shapeComponent.Width = max.X - min.X;
        _shapeComponent.Height = max.Y - min.Y;
        
        foreach (var entity in Context.Entities)
        {
            var positionComponent = entity.GetComponent<Position>()?.Component;
            var selectableComponent = entity.GetComponent<Selectable>()?.Component;

            if (positionComponent is null || selectableComponent is null)
            {
                continue;
            }

            var relativePosition = positionComponent.Value - _positionComponent.Value;
            selectableComponent.Selected = _shapeComponent.Contains(relativePosition);
        }
    }
}