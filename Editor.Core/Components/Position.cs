using System.ComponentModel;
using System.Numerics;
using Editor.Component;

namespace Editor.Core.Components;

public class Position : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    public Vector2 Value { get; set; }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
}