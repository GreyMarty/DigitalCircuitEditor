using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;

namespace Editor.Core.Rendering.Adapters;

public class RectShapeToRendererAdapter : EditorComponentBase
{
    private RectangleShape _shapeComponent = default!;
    private RectangleRenderer _rectangleRenderer = default!;
    
    
    protected override void OnInit()
    {
        _shapeComponent = Entity.GetRequiredComponent<RectangleShape>()!;
        _rectangleRenderer = Entity.GetRequiredComponent<RectangleRenderer>()!;
        
        _shapeComponent.PropertyChanged += ShapeComponent_OnPropertyChanged;
    }

    protected override void OnDestroy()
    {
        _shapeComponent.PropertyChanged -= ShapeComponent_OnPropertyChanged;
    }
    
    private void ShapeComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _rectangleRenderer.Width = _shapeComponent.Width;
        _rectangleRenderer.Height = _shapeComponent.Height;
    }
}