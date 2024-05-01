using Editor.Core.Rendering.Helpers;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class LabeledCircleRenderer : LabeledShapeRenderer
{
    public float Radius { get; set; }
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        canvas.DrawCircle(0,0, Radius, FillPaint);
        canvas.DrawCircle(0, 0, Radius, StrokePaint);

        if (string.IsNullOrWhiteSpace(Text))
        {
            return;
        }
        
        var oldColor = FillPaint.Color;
        FillPaint.Color = StrokePaint.Color;
            
        TextHelper.DrawText(camera, canvas, Text, Font, FillPaint, Anchor);
            
        FillPaint.Color = oldColor;
    }
}