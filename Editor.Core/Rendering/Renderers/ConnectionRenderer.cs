using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class ConnectionRenderer : ShapeRenderer
{
    private readonly SKFont _font = new SKFont(SKTypeface.FromFamilyName("Consolas"), 1);
    
    private Connection _connectionComponent = default!;
    private ChildOf _childOfComponent = default!;


    public float FontSize
    {
        get => _font.Size;
        set => _font.Size = value;
    }
    
    
    protected override void OnInit()
    {
        base.OnInit();

        _connectionComponent = Entity.GetRequiredComponent<Connection>()!;
        _childOfComponent = Entity.GetRequiredComponent<ChildOf>()!;
    }

    protected override void OnRender(Camera camera, SKCanvas canvas)
    {
        if (_connectionComponent.Target is null)
        {
            return;
        }
     
        var targetPosition = _connectionComponent.Target.GetRequiredComponent<Position>()!.Component!;

        var sourceShape = _childOfComponent.Parent?.GetComponent<Shape>()?.Component;
        var targetShape = _connectionComponent.Target.GetComponent<Shape>()?.Component;
        
        var from = Vector2.Zero;
        var to = targetPosition.Value - Position;

        if (sourceShape is not null)
        {
            from += sourceShape.NearestIntersection(to - from);
        }

        if (targetShape is not null)
        {
            to += targetShape.NearestIntersection(from - to);
        }

        var delta = to - from;
        var distance = delta.Length();
        var rotation = MathF.Atan2(delta.Y, delta.X);

        canvas.Save();
        canvas.Translate(from.X, from.Y);
        canvas.RotateRadians(rotation);
        
        canvas.DrawLine(0, 0, distance, 0, StrokePaint);

        var text = _connectionComponent.Label;

        const float scaleFactor = 10;
        
        if (!string.IsNullOrWhiteSpace(text))
        {
            _font.Size *= scaleFactor;
            _font.MeasureText(text.Select(c => (ushort)c).ToArray(), out var rect);
            
            var origin = new SKPoint(
                -rect.Left - rect.Width / 2,
                -rect.Top - rect.Height / 2
            );
            
            using var textBlob = SKTextBlob.Create(text, _font, origin);

            canvas.Save();
            
            if (Math.Abs(rotation) > Math.PI / 2)
            {
                canvas.Translate(distance / 2, _font.Size / scaleFactor);
                canvas.RotateDegrees(180);
            }
            else 
            {
                canvas.Translate(distance / 2, -_font.Size / scaleFactor);
            }
            
            canvas.Scale(1 / scaleFactor);

            var oldColor = FillPaint.Color;
            FillPaint.Color = StrokePaint.Color;
            canvas.DrawText(textBlob, 0, 0, FillPaint);
            FillPaint.Color = oldColor;
            
            canvas.Restore();
            _font.Size /= scaleFactor;
        }
        
        canvas.Restore();
    }
}