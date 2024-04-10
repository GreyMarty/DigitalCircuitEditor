using System.ComponentModel;
using System.Numerics;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Extensions;
using Editor.Core.Shapes;

namespace Editor.Core.Adapters;

public class ConnectionToLineShapeAdapter : EditorComponentBase
{
    private Position _positionComponent = default!;
    private Connection _connectionComponent = default!;
    private ChildOf _childOfComponent = default!;
    private LineShape _shapeComponent = default!;
    private Position? _targetPosition;
    
    protected override void OnInit()
    {
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
        _connectionComponent = Entity.GetRequiredComponent<Connection>()!;
        _childOfComponent = Entity.GetRequiredComponent<ChildOf>()!;
        _shapeComponent = Entity.GetRequiredComponent<LineShape>()!;
        
        _connectionComponent.PropertyChanged += ConnectionComponent_OnPropertyChanged;
        _positionComponent.PropertyChanged += Position_OnPropertyChanged;
        ConnectionComponent_OnPropertyChanged(this, new PropertyChangedEventArgs(null));
    }

    protected override void OnDestroy()
    {
        _connectionComponent.PropertyChanged -= ConnectionComponent_OnPropertyChanged;

        if (_targetPosition is not null)
        {
            _targetPosition.PropertyChanged -= Position_OnPropertyChanged;
        }
    }

    private void ConnectionComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _targetPosition = _targetPosition.Rebind(
            _connectionComponent.Target?.GetRequiredComponent<Position>()!,
            Position_OnPropertyChanged
        );
        
        Position_OnPropertyChanged(this, new PropertyChangedEventArgs(null));
    }

    private void Position_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var sourceShape = _childOfComponent.Parent?.GetComponent<Shape>()?.Component;
        var targetShape = _connectionComponent.Target?.GetComponent<Shape>()?.Component;
        
        var from = Vector2.Zero;
        var to = (_targetPosition?.Value ?? Vector2.Zero) - _positionComponent.Value;
        
        if (sourceShape is not null)
        {
            from += sourceShape.NearestIntersection(to - from);
        }
        
        if (targetShape is not null)
        {
            to += targetShape.NearestIntersection(from - to);
        }
        
        _shapeComponent.Start = from;
        _shapeComponent.End = to;
    }
}