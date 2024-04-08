using System.Numerics;
using Editor.Core.Components;
using Editor.Core.Rendering.Helpers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public class ConnectionRenderer : LabeledShapeRenderer
{
    private Connection _connectionComponent = default!;
    private ChildOf _childOfComponent = default!;
    
    
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