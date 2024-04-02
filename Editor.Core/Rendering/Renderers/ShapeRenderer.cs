using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public abstract class ShapeRenderer : Renderer
{
    public SKColor Stroke
    {
        get => StrokePaint.Color;
        set => StrokePaint.Color = value;
    }

    public float StrokeThickness
    {
        get => StrokePaint.StrokeWidth;
        set => StrokePaint.StrokeWidth = value;
    }

    public SKColor Fill
    {
        get => FillPaint.Color;
        set => FillPaint.Color = value;
    }
 
    protected SKPaint StrokePaint { get; } = new()
    {
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
    };

    protected SKPaint FillPaint { get; } = new()
    {
        IsAntialias = true,
        Style = SKPaintStyle.Fill
    };

    protected override void OnDestroy()
    {
        StrokePaint.Dispose();
        FillPaint.Dispose();
        base.OnDestroy();
    }
}