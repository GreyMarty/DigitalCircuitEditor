using System.Numerics;

namespace Editor.Core.Shapes;

public class CircleShape : Shape
{
    public float Radius { get; init; }
    
    
    public override bool Contains(Vector2 point) => Vector2.DistanceSquared(point, Vector2.Zero) < Radius * Radius;

    public override Vector2 NearestIntersection(Vector2 directionFromCenter)
    {
        var normalized = directionFromCenter / directionFromCenter.Length();
        return normalized * Radius;
    }
}