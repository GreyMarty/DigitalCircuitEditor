using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;

namespace Editor.Core.Adapters;

public class LineShapeToRendererAdapter : EditorComponentBase
{
    private LabeledLineRenderer _rendererComponent = default!;

    protected override void OnInit()
    {
        _rendererComponent = Entity.GetRequiredComponent<LabeledLineRenderer>()!;
        
        Entity.ComponentChanged += Entity_OnComponentChanged;
        OnShapeComponentChanged(Entity.GetRequiredComponent<LineShape>()!);
    }

    protected override void OnDestroy()
    {
        Entity.ComponentChanged -= Entity_OnComponentChanged;
    }

    private void OnShapeComponentChanged(LineShape shapeComponent)
    {
        _rendererComponent.Start = shapeComponent.Start;
        _rendererComponent.End = shapeComponent.End;
    }
    
    private void Entity_OnComponentChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is LineShape component)
        {
            OnShapeComponentChanged(component);
        }
    }
}