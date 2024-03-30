using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Editor.Core.Wpf.Converters;

public class ColorToBrushConverter : IValueConverter
{
    public ResourceDictionary ResourceDictionary { get; set; } = default!;
    
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ResourceDictionary[$"{value}Brush"]!;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}