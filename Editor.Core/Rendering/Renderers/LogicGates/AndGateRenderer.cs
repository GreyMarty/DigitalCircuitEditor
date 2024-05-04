using SkiaSharp;

namespace Editor.Core.Rendering.Renderers.LogicGates;

public class AndGateRenderer : ShapeRenderer
{
    public float Size { get; set; }
    
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        var halfSize = Size / 2;
        
        using var path = new SKPath();
        path.MoveTo(-halfSize, -halfSize);
        path.LineTo(0, -halfSize);
        path.ArcTo(new SKRect(-halfSize, -halfSize, halfSize, halfSize), -90, 180, true);
        path.LineTo(-halfSize, halfSize);
        path.LineTo(-halfSize, -halfSize);
        path.Close();
        
        canvas.DrawPath(path, FillPaint);
        canvas.DrawPath(path, StrokePaint);
    }
}