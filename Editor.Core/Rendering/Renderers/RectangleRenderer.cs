using System.Numerics;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class RectangleRenderer : ShapeRenderer
{
    public float Width { get; set; }
    public float Height { get; set; }
    
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        var topLeft = new Vector2(-Width / 2, -Height / 2);
        
        canvas.DrawRect(topLeft.X, topLeft.Y, Width, Height, FillPaint);
        canvas.DrawRect(topLeft.X, topLeft.Y, Width, Height, StrokePaint);
    }
}