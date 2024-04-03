using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Events;
using Editor.Core.Shapes;
using TinyMessenger;

namespace Editor.Core.Components;

public class Hoverable : EditorComponentBase
{
    private Position _positionComponent = default!;
    private Shape _shapeComponent = default!;
    
    private IEventBusSubscriber _eventBus = default!;
    
    
    public bool Hovered { get; set; }


    protected override void OnInit()
    {
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
        _shapeComponent = Entity.GetRequiredComponent<Shape>()!;

        _eventBus = Context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseMove>(OnMouseMove);
    }

    protected override void OnDestroy()
    {
        _eventBus.Unsubscribe<MouseMove>();
    }

    private void OnMouseMove(MouseMove e)
    {
        var mousePosition = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels);
        var relativeMousePosition = mousePosition - _positionComponent.Value;
        
        Hovered = _shapeComponent.Contains(relativeMousePosition);
    }
}