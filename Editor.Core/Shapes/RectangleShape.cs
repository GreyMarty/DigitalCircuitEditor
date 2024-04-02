using System.Numerics;

namespace Editor.Core.Shapes;

public class RectangleShape : Shape
{
    public float Width { get; set; }
    public float Height { get; set; }
    
    
    public override bool Contains(Vector2 point)
    {
        return point.X >= -Width / 2 &&
               point.X <=  Width / 2 &&
               point.Y >= -Height / 2 &&
               point.Y <=  Height / 2;
    }

    public override Vector2 NearestIntersection(Vector2 directionFromCenter)
    {
        var halfWidth = Width / 2;
        var halfHeight = Height / 2;

        directionFromCenter /= directionFromCenter.Length();

        var tx = halfWidth / Math.Abs(directionFromCenter.X);
        var ty = halfHeight / Math.Abs(directionFromCenter.Y);

        var t = Math.Min(tx, ty);

        return directionFromCenter * t;
    }
}