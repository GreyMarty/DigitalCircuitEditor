using System.ComponentModel;

namespace Editor.Core.ViewModels;

public static class PropertyChanged
{
    public static T? Rebind<T>(this T? from, T? to, PropertyChangedEventHandler propertyChanged)
        where T : INotifyPropertyChanged
    {
        if (from is not null)
        {
            from.PropertyChanged -= propertyChanged;
        }

        if (to is not null)
        {
            to.PropertyChanged += propertyChanged;
        }

        return to;
    }
}