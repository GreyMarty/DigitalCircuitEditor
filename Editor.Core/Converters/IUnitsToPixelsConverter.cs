using System.Numerics;

namespace Editor.Core.Converters;

public interface IUnitsToPixelsConverter
{
    public Vector2 ToPixels(Vector2 positionUnits);
    public Vector2 ToUnits(Vector2 positionPixels);
}