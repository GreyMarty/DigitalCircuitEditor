using System.ComponentModel;
using System.Runtime.CompilerServices;
using Editor.Component;

namespace Editor.Core.Components;

public class Connection : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    public Entity? Source { get; set; }
    public Entity? Target { get; set; }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
}
