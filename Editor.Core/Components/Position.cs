using System.ComponentModel;
using System.Numerics;
using Editor.Component;
using Editor.Core.Converters;
using PropertyChanged;

namespace Editor.Core.Components;

public class Position : EditorComponentBase
{
    private ComponentRef<Position>? _parentRef;
    
        
    public Vector2 Local { get; set; }

    public Vector2 Value
    {
        get => Local + (_parentRef?.Component?.Value ?? Vector2.Zero);
        set => Local = value - (_parentRef?.Component?.Value ?? Vector2.Zero);
    }
    
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
  

    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _parentRef = entity.GetComponent<ChildOf>()?.Component?.Parent?.GetRequiredComponent<Position>();

        if (_parentRef?.Component is not null)
        {
            _parentRef.Component.PropertyChanged += Parent_OnPropertyChanged;
        }
    }

    protected override void OnDestroy()
    {
        if (_parentRef?.Component is not null)
        {
            _parentRef.Component.PropertyChanged -= Parent_OnPropertyChanged;
        }
    }

    private void Parent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Value));
        OnPropertyChanged(nameof(X));
        OnPropertyChanged(nameof(Y));
    }
}