using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace Editor.Core.Wpf.Converters;

public class VectorToPointConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((Vector2)value).ToPoint();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}