using System.Numerics;
using Editor.Core.Components;

namespace Editor.Core.Shapes;

public interface IShape
{
    public bool Contains(Vector2 point);
    public abstract Vector2 NearestIntersection(Vector2 directionFromCenter);
}

public abstract class Shape : EditorComponentBase, IShape
{
    public abstract bool Contains(Vector2 point);
    public abstract Vector2 NearestIntersection(Vector2 directionFromCenter);
}