using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;

namespace Editor.Core.Adapters;

public class RectShapeToRendererAdapter : EditorComponentBase
{
    private RectangleRenderer _rectangleRenderer = default!;
    
    
    protected override void OnInit()
    {
        _rectangleRenderer = Entity.GetRequiredComponent<RectangleRenderer>()!;
     
        Entity.ComponentChanged += Entity_OnComponentChanged;
    }

    protected override void OnDestroy()
    {
        Entity.ComponentChanged -= Entity_OnComponentChanged;
    }

    private void OnShapeComponentChanged(RectangleShape shapeComponent)
    {
        _rectangleRenderer.Width = shapeComponent.Width;
        _rectangleRenderer.Height = shapeComponent.Height;
    }
    
    private void Entity_OnComponentChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is RectangleShape component)
        {
            OnShapeComponentChanged(component);
        }
    }
}