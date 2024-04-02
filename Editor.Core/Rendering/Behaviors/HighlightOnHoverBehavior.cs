using System.ComponentModel;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Rendering.Behaviors;

public class HighlightOnHoverBehavior : EditorComponentBase
{
    private Hoverable _hoverableComponent = default!;
    private ShapeRenderer _shapeRenderer = default!;
    
    private SKColor _defaultFill;
    
    
    public SKColor HighlightColor { get; set; }
    
    
    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _hoverableComponent = entity.GetRequiredComponent<Hoverable>()!;
        _shapeRenderer = entity.GetRequiredComponent<ShapeRenderer>()!;

        _defaultFill = _shapeRenderer.Fill;
        
        _hoverableComponent.PropertyChanged += HoverableComponent_OnPropertyChanged;
    }

    protected override void OnDestroy()
    {
        _hoverableComponent.PropertyChanged -= HoverableComponent_OnPropertyChanged;
    }
    
    private void HoverableComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _shapeRenderer.Fill = _hoverableComponent.Hovered ? HighlightColor : _defaultFill;
    }
}