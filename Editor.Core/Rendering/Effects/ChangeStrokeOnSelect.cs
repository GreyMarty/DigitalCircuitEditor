using System.ComponentModel;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Rendering.Effects;

public class ChangeStrokeOnSelect : EditorComponentBase
{
    private Selectable _selectableComponent = default!;
    private ShapeRenderer _shapeRenderer = default!;
    
    private SKColor _defaultStroke;
    private float _defaultStrokeThickness;
    
    
    public SKColor HighlightStroke { get; set; } = SKColors.CornflowerBlue;
    public float HighlightStrokeThickness { get; set; } = 0.3f;
    
    
    protected override void OnInit()
    {
        _selectableComponent = Entity.GetRequiredComponent<Selectable>()!;
        _shapeRenderer = Entity.GetRequiredComponent<ShapeRenderer>()!;

        _defaultStroke = _shapeRenderer.Stroke;
        _defaultStrokeThickness = _shapeRenderer.StrokeThickness;
        
        _selectableComponent.PropertyChanged += SelectableComponent_OnPropertyChanged;
    }

    protected override void OnDestroy()
    {
        _selectableComponent.PropertyChanged -= SelectableComponent_OnPropertyChanged;
    }
    
    private void SelectableComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_selectableComponent.Selected)
        {
            _shapeRenderer.Stroke = HighlightStroke;
            _shapeRenderer.StrokeThickness = HighlightStrokeThickness;
            return;
        }
        
        _shapeRenderer.Stroke = _defaultStroke;
        _shapeRenderer.StrokeThickness = _defaultStrokeThickness;
    }
}