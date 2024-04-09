using System.ComponentModel;

namespace Editor.Core.Extensions;

public static class PropertyChangedExtensions
{
    public static T? Rebind<T>(this T? field, T? value, PropertyChangedEventHandler propertyChanged)
        where T : INotifyPropertyChanged
    {
        if (field is not null)
        {
            field.PropertyChanged -= propertyChanged;
        }

        field = value;

        if (field is not null)
        {
            field.PropertyChanged += propertyChanged;
        }

        return field;
    }
}