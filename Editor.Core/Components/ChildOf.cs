using System.ComponentModel;
using Editor.Component;

namespace Editor.Core.Components;

public class ChildOf : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    public Entity? Parent { get; set; }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}