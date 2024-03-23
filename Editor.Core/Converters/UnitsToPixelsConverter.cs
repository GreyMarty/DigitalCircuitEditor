using System.Numerics;

namespace Editor.Core.Converters;

public class UnitsToPixelsConverter : IUnitsToPixelsConverter
{
    private static readonly UnitsToPixelsConverter Instance = new();
    
    private UnitsToPixelsConverter()
    {
    }


    public static IUnitsToPixelsConverter Default => Instance;
    
    
    public Vector2 ToPixels(Vector2 positionUnits) => positionUnits;
    public Vector2 ToUnits(Vector2 positionPixels) => positionPixels;
}