using System.ComponentModel;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;

namespace Editor.Core.Rendering.Adapters;

public class RectangleShapeToRendererAdapter : EditorComponentBase
{
    private RectangleShape _shapeComponent = default!;
    private RectangleRenderer _rectangleRenderer = default!;
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _shapeComponent = entity.GetRequiredComponent<RectangleShape>()!;
        _rectangleRenderer = entity.GetRequiredComponent<RectangleRenderer>()!;
        
        _shapeComponent.PropertyChanged += ShapeComponent_OnPropertyChanged;
    }

    protected override void OnDestroy()
    {
    }
    
    private void ShapeComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _rectangleRenderer.Width = _shapeComponent.Width;
        _rectangleRenderer.Height = _shapeComponent.Height;
    }
}