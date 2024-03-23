using System.ComponentModel;
using System.Runtime.CompilerServices;
using Editor.Component;
using Editor.Core;

namespace Editor.ViewModel;

public class ViewModelComponentBase : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}