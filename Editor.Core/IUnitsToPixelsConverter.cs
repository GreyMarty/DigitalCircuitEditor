using System.Numerics;

namespace Editor.Core;

public interface IUnitsToPixelsConverter
{
    public Vector2 Convert(Vector2 position);
}