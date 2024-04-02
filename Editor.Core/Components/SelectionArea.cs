using System.Numerics;
using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Shapes;
using TinyMessenger;

namespace Editor.Core.Components;

public class SelectionArea : EditorComponentBase
{
    private EditorContext _context = default!;
    private IEntity _entity = default!;

    private Position _positionComponent = default!;
    private RectangleShape _shapeComponent = default!;

    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken _mouseMoveToken = default!;
    private TinyMessageSubscriptionToken _mouseUpToken = default!;


    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _context = context;
        _entity = entity;

        _positionComponent = entity.GetRequiredComponent<Position>().Component!;
        _shapeComponent = entity.GetRequiredComponent<RectangleShape>().Component!;

        _eventBus = context.EventBus;
        _mouseMoveToken = _eventBus.Subscribe<MouseMove>(OnMouseMove);
        _mouseUpToken = _eventBus.Subscribe<MouseButtonUp>(OnMouseUp);

        _context.MouseLocked = true;
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseMove>(_mouseMoveToken);
        _eventBus.Unsubscribe<MouseButtonUp>(_mouseUpToken);

        _context.MouseLocked = false;
    }

    private void OnMouseMove(MouseMove e)
    {
        var halfSize = new Vector2(_shapeComponent.Width / 2, _shapeComponent.Height / 2);
        var min = _positionComponent.Value - halfSize;
        var max = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels);

        _positionComponent.Value = (min + max) / 2;
        _shapeComponent.Width = max.X - min.X;
        _shapeComponent.Height = max.Y - min.Y;
        
        foreach (var entity in _context.Entities)
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

    private void OnMouseUp(MouseButtonUp e)
    {
        _context.Destroy(_entity);
    }
}