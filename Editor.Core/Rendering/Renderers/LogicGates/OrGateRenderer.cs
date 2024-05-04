using SkiaSharp;

namespace Editor.Core.Rendering.Renderers.LogicGates;

public class OrGateRenderer : ShapeRenderer
{
    public float Size { get; set; }
    
    
    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        var halfSize = Size / 2;

        var offset1 = Size / 4;
        var angle1 = MathF.Atan(Size / (halfSize + offset1)) / MathF.PI * 180;

        var offset2 = halfSize * MathF.Sqrt(3);
        var angle2 = 30;

        var offset3Y = halfSize / 2;
        var offset3X = Size * MathF.Cos(MathF.Asin(offset3Y / Size)) - offset2;
        
        using var path = new SKPath();
        path.MoveTo(-halfSize, -halfSize);
        path.ArcTo(new SKRect(-1.5f * Size - offset1, -halfSize, halfSize + offset1, 1.5f * Size + 2 * offset1), -90, angle1, false);
        path.ArcTo(new SKRect(-1.5f * Size - offset1, -1.5f * Size - 2 * offset1, halfSize + offset1, halfSize), 90 - angle1, angle1, false);
        path.ArcTo(new SKRect(-1.5f * Size - offset2, -Size, halfSize - offset2, Size), angle2, -2 * angle2, false);
        path.Close();
        
        path.MoveTo(-halfSize, -offset3Y);
        path.LineTo(-halfSize + offset3X, -offset3Y);
        
        path.MoveTo(-halfSize, offset3Y);
        path.LineTo(-halfSize + offset3X, offset3Y);
        
        canvas.DrawPath(path, FillPaint);
        canvas.DrawPath(path, StrokePaint);
    }
}