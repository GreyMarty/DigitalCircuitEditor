using System.Numerics;
using System.Windows;

namespace Editor.Core.Wpf.Converters;

public static class PointToVectorConverter
{
    public static Vector2 ToVector2(this Point point)
    {
        return new Vector2((float)point.X, (float)point.Y);
    }
    
    public static Point ToPoint(this Vector2 vector)
    {
        return new Point(vector.X, vector.Y);
    }
}