using System.ComponentModel;
using System.Runtime.CompilerServices;
using Editor.Component;

namespace Editor.Core.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;


    public virtual void Init(EditorWorld world, IEntity entity) { }
    
    public virtual void Dispose() { }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}