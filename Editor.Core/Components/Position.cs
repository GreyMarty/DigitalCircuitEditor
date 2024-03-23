using System.ComponentModel;
using System.Numerics;
using Editor.Component;

namespace Editor.Core.Components;

public class Position : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    public Vector2 Value { get; set; }

    public float X
    {
        get => Value.X;
        set => Value = Value with { X = value };
    }

    public float Y
    {
        get => Value.Y;
        set => Value = Value with { Y = value };
    }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
}