using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class CircleRenderer : ShapeRenderer
{
    public float Radius { get; set; }
    
    
    public override void Render(Camera camera, SKCanvas canvas)
    {
        canvas.DrawCircle(Position.X, Position.Y, Radius, FillPaint);
        canvas.DrawCircle(Position.X, Position.Y, Radius, StrokePaint);
    }
}