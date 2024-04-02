using System.Numerics;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class RectangleRenderer : ShapeRenderer
{
    public float Width { get; set; }
    public float Height { get; set; }
    
    
    public override void Render(Camera camera, SKCanvas canvas)
    {
        var center = Position - new Vector2(Width / 2, Height / 2);
        
        canvas.DrawRect(center.X, center.Y, Width, Height, FillPaint);
        canvas.DrawRect(center.X, center.Y, Width, Height, StrokePaint);
    }
}