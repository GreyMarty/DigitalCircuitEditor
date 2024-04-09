using System.Numerics;
using Editor.Core.Rendering.Helpers;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class LabeledLineRenderer : LabeledShapeRenderer
{
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }

    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        var delta = End - Start;
        var distance = delta.Length();
        var rotation = MathF.Atan2(delta.Y, delta.X);

        canvas.Save();
        canvas.Translate(Start.X, Start.Y);
        canvas.RotateRadians(rotation);
        
        canvas.DrawLine(0, 0, distance, 0, StrokePaint);
        
        if (!string.IsNullOrWhiteSpace(Text))
        {
            canvas.Save();
            
            if (Math.Abs(rotation) > Math.PI / 2)
            {
                canvas.Translate(distance / 2, Font.Size);
                canvas.RotateDegrees(180);
            }
            else 
            {
                canvas.Translate(distance / 2, -Font.Size);
            }

            var oldColor = FillPaint.Color;
            FillPaint.Color = StrokePaint.Color;
            
            RenderingHelper.DrawText(camera, canvas, Text, Font, FillPaint, Anchor);
            
            FillPaint.Color = oldColor;
            
            canvas.Restore();
        }
        
        canvas.Restore();
    }
}