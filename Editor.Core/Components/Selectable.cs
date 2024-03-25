using System.ComponentModel;
using Editor.Component;

namespace Editor.Core.Components;

public class Selectable : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    public bool Selected { get; set; }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
}
