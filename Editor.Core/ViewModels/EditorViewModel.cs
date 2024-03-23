using System.ComponentModel;
using System.Numerics;
using CommunityToolkit.Mvvm.Input;
using Editor.Core.Converters;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.ViewModels;

public partial class EditorViewModel : INotifyPropertyChanged
{
    private readonly IUnitsToPixelsConverter _positionConverter;
    
    
     public Vector2 Offset { get; set; }
    
    public float OffsetX
    {
        get => Offset.X;
        set => Offset = Offset with { X = value };
    }
    
    public float OffsetY
    {
        get => Offset.Y;
        set => Offset = Offset with { Y = value };
    }

    public float Scale { get; set; } = 1;
    
    
    public event PropertyChangedEventHandler? PropertyChanged;


    public EditorViewModel(EditorWorld world)
    {
        _positionConverter = world.PositionConverter;
    }
    

    [RelayCommand]
    private void OnMouseMove(MouseMove e)
    {
        if (e.Button != MouseButton.Middle)
        {
            return;
        }

        Offset += _positionConverter.ToPixels(e.NewPosition - e.OldPosition);
    }

    [RelayCommand]
    private void OnMouseWheel(MouseWheel e)
    {
        Scale *= e.Delta > 0 ? 1.1f : 1 / 1.1f;
    }
}