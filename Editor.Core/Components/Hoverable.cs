using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Editor.Component;

namespace Editor.Core.Components;

public partial class Hoverable : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    public bool Hovered { get; private set; }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    
    [RelayCommand]
    private void OnHover(bool hovered)
    {
        Hovered = hovered;
    }
}