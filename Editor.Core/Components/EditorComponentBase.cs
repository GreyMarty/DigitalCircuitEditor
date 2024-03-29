using System.ComponentModel;
using System.Runtime.CompilerServices;
using Editor.Component;

namespace Editor.Core.Components;

public class EditorComponentBase : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}