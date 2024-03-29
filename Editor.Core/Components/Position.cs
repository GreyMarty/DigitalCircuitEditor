using System.ComponentModel;
using System.Numerics;
using Editor.Component;
using Editor.Core.Converters;

namespace Editor.Core.Components;

public class Position : EditorComponentBase
{
    private IUnitsToPixelsConverter _converter = default!;
    private ComponentRef<Position>? _parent;
    
        
    public Vector2 Local { get; set; }

    public Vector2 Value
    {
        get => Local + (_parent?.Component?.Value ?? Vector2.Zero);
        set => Local = value - (_parent?.Component?.Value ?? Vector2.Zero);
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

    public Vector2 ValuePixels
    {
        get => _converter.ToPixels(Value);
        set => Value = _converter.ToUnits(value);
    }

    public float PixelsX
    {
        get => ValuePixels.X;
        set => ValuePixels = ValuePixels with { X = value };
    }
    
    public float PixelsY
    {
        get => ValuePixels.Y;
        set => ValuePixels = ValuePixels with { Y = value };
    }


    public override void Init(EditorWorld world, IEntity entity)
    {
        _converter = world.PositionConverter;
        _parent = entity.GetComponent<ChildOf>()?.Component?.Parent?.GetRequiredComponent<Position>();

        if (_parent?.Component is not null)
        {
            _parent.Component.PropertyChanged += Parent_OnPropertyChanged;
        }
    }

    public override void Dispose()
    {
        if (_parent?.Component is not null)
        {
            _parent.Component.PropertyChanged -= Parent_OnPropertyChanged;
        }
        
        base.Dispose();
    }

    private void Parent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Value));
    }
}