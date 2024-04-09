using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;

namespace Editor.Core.Adapters;

public class LineShapeToRendererAdapter : EditorComponentBase
{
    private LineShape _shapeComponent = default!;
    private LabeledLineRenderer _rendererComponent = default!;

    protected override void OnInit()
    {
        _shapeComponent = Entity.GetRequiredComponent<LineShape>()!;
        _rendererComponent = Entity.GetRequiredComponent<LabeledLineRenderer>()!;
        
        _shapeComponent.PropertyChanged += ShapeComponent_OnPropertyChanged;
        ShapeComponent_OnPropertyChanged(this, new PropertyChangedEventArgs(null));
    }

    protected override void OnDestroy()
    {
        _shapeComponent.PropertyChanged -= ShapeComponent_OnPropertyChanged;
    }
    
    private void ShapeComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _rendererComponent.Start = _shapeComponent.Start;
        _rendererComponent.End = _shapeComponent.End;
    }
}