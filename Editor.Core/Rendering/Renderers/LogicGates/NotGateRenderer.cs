using SkiaSharp;

namespace Editor.Core.Rendering.Renderers.LogicGates;

public class NotGateRenderer : ShapeRenderer
{
    public float Size { get; set; }
    
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        var halfSize = Size / 2;
        
        using var path = new SKPath();
        path.MoveTo(-halfSize, -halfSize);
        path.LineTo(halfSize, 0);
        path.LineTo(-halfSize, halfSize);
        path.Close();
        
        canvas.DrawPath(path, FillPaint);
        canvas.DrawPath(path, StrokePaint);
        
        canvas.DrawOval(halfSize, 0, Size * 0.15f, Size * 0.15f, FillPaint);
        canvas.DrawOval(halfSize, 0, Size * 0.15f, Size * 0.15f, StrokePaint);
    }
}