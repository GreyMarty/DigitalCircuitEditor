using System.Numerics;

namespace Editor.Core.Converters;

public class UnitsToPixelsConverter : IUnitsToPixelsConverter
{
    private static readonly UnitsToPixelsConverter Instance = new();

    private readonly float _unit = 1;
    
    
    private UnitsToPixelsConverter(float unit = 1)
    {
    }

    public static IUnitsToPixelsConverter Default => Instance;

    public static IUnitsToPixelsConverter FromUnit(float unit) => new UnitsToPixelsConverter(unit);
    
    
    public Vector2 ToPixels(Vector2 positionUnits) => positionUnits * _unit;
    public Vector2 ToUnits(Vector2 positionPixels) => positionPixels / _unit;
}