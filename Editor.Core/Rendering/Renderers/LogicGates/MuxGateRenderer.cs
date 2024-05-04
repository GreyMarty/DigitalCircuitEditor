using SkiaSharp;

namespace Editor.Core.Rendering.Renderers.LogicGates;

public class MuxGateRenderer : ShapeRenderer
{
    public float Size { get; set; }
    
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        var backHeight = Size * 1.5f;
        var halfBackHeight = backHeight / 2;

        var frontHeight = Size;
        var halfFrontHeight = frontHeight / 2;
        
        var halfSize = Size / 2;
        
        using var path = new SKPath();
        path.MoveTo(-halfSize, -halfBackHeight);
        path.LineTo(halfSize, -halfFrontHeight);
        path.LineTo(halfSize, halfFrontHeight);
        path.LineTo(-halfSize, halfBackHeight);
        path.Close();
        
        path.MoveTo(0, halfBackHeight);
        path.LineTo(0, halfBackHeight - (halfBackHeight - halfFrontHeight) / 2);
        
        canvas.DrawPath(path, FillPaint);
        canvas.DrawPath(path, StrokePaint);
    }
}