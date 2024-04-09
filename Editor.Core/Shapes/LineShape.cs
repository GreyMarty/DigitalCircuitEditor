using System.Numerics;

namespace Editor.Core.Shapes;

public class LineShape : Shape
{
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }
    public float Thickness { get; set; }
    
    public override bool Contains(Vector2 point)
    {
        var segment = End - Start;
        var v = point - Start;
        var t = Vector2.Dot(v, segment) / Vector2.Dot(segment, segment);

        var distance = t switch
        {
            < 0 => Vector2.Distance(point, Start),
            > 1 => Vector2.Distance(point, End),
            _ => Vector2.Distance(point, Start + t * segment)
        };

        return distance <= Thickness;
    }

    public override Vector2 NearestIntersection(Vector2 directionFromCenter)
    {
        return Vector2.Zero;
    }
}