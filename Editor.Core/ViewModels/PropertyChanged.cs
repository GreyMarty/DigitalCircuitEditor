using System.ComponentModel;
using Editor.Component;

namespace Editor.Core.ViewModels;

public static class PropertyChanged
{
    public static ComponentRef<T>? Rebind<T>(this ComponentRef<T>? from, ComponentRef<T>? to, PropertyChangedEventHandler propertyChanged)
        where T : ComponentBase, INotifyPropertyChanged
    {
        if (from?.Component is not null)
        {
            from.Component.PropertyChanged -= propertyChanged;
        }

        if (to?.Component is not null)
        {
            to.Component.PropertyChanged += propertyChanged;
        }

        return to;
    }
}