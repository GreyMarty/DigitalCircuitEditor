using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class CircleRenderer : ShapeRenderer
{
    public float Radius { get; set; }
    
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        canvas.DrawCircle(0,0, Radius, FillPaint);
        canvas.DrawCircle(0, 0, Radius, StrokePaint);
    }
}