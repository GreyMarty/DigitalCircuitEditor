using System.Numerics;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class GridRenderer : ShapeRenderer
{
    public float MajorStep { get; set; }
    public float Subdivisions { get; set; }
    
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        var majorStep = MajorStep * camera.Scale;
        var minorStep = majorStep / Subdivisions;
        
        var halfWidth = camera.Size.X / 2;
        var halfHeight = camera.Size.Y / 2;
        
        var majorOffset = new Vector2(
            camera.Position.X % majorStep,
            camera.Position.Y % majorStep
        );
        
        var minorOffset = new Vector2(
            camera.Position.X % minorStep,
            camera.Position.Y % minorStep
        );

        var baseStrokeThickness = StrokeThickness;

        StrokeThickness = baseStrokeThickness / 2 / camera.PixelsPerUnit;
        RenderGrid(canvas, minorStep, 1, minorOffset, halfWidth, halfHeight);
        RenderGrid(canvas, minorStep, -1, minorOffset - minorStep * Vector2.One, halfWidth, halfHeight);

        StrokeThickness = baseStrokeThickness / camera.PixelsPerUnit;
        RenderGrid(canvas, majorStep, 1, majorOffset, halfWidth, halfHeight);
        RenderGrid(canvas, majorStep, -1, majorOffset - majorStep * Vector2.One, halfWidth, halfHeight);

        StrokeThickness = baseStrokeThickness;
    }

    private void RenderGrid(SKCanvas canvas, float step, float sign, Vector2 offset, float halfWidth, float halfHeight)
    {
        for (var x = 0f; x < halfWidth; x += step)
        {
            canvas.DrawLine(x * sign + offset.X, -halfHeight, x * sign + offset.X, halfHeight, StrokePaint);
        }
            
        for (var y = 0f; y < halfHeight; y += step)
        {
            canvas.DrawLine(-halfWidth, y * sign + offset.Y, halfWidth, y * sign + offset.Y, StrokePaint);
        }
    }
}