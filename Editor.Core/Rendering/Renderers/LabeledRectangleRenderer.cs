using System.Numerics;
using Editor.Core.Rendering.Helpers;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class LabeledRectangleRenderer : LabeledShapeRenderer
{
    public float Width { get; set; }
    public float Height { get; set; }
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        var topLeft = new Vector2(-Width / 2, -Height / 2);
        
        canvas.DrawRect(topLeft.X, topLeft.Y, Width, Height, FillPaint);
        canvas.DrawRect(topLeft.X, topLeft.Y, Width, Height, StrokePaint);
        
        if (string.IsNullOrWhiteSpace(Text))
        {
            return;
        }
        
        var oldColor = FillPaint.Color;
        FillPaint.Color = StrokePaint.Color;
            
        RenderingHelper.DrawText(camera, canvas, Text, Font, FillPaint, Anchor);
            
        FillPaint.Color = oldColor;
    }
}