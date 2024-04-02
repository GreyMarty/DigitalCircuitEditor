using Editor.Component;
using Editor.Core.Events;
using Editor.Core.Shapes;
using TinyMessenger;

namespace Editor.Core.Components;

public class Hoverable : EditorComponentBase
{
    private Position _positionComponent = default!;
    private Shape _shapeComponent = default!;
    
    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken? _mouseMoveToken;
    
    
    public bool Hovered { get; set; }


    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _positionComponent = entity.GetRequiredComponent<Position>()!;
        _shapeComponent = entity.GetRequiredComponent<Shape>()!;

        _eventBus = context.EventBus;
        _mouseMoveToken = _eventBus.Subscribe<MouseMove>(OnMouseMove);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseMove>(_mouseMoveToken!);
    }

    private void OnMouseMove(MouseMove e)
    {
        var mousePosition = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels);
        var relativeMousePosition = mousePosition - _positionComponent.Value;
        
        Hovered = _shapeComponent.Contains(relativeMousePosition);
    }
}