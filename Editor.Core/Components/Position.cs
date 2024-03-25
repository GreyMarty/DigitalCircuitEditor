﻿using System.ComponentModel;
using System.Numerics;
using Editor.Component;
using Editor.Core.Converters;

namespace Editor.Core.Components;

public class Position : ComponentBase<EditorWorld>, INotifyPropertyChanged
{
    private IUnitsToPixelsConverter _converter = default!;
    private Position? _parent;
    
        
    public Vector2 Local { get; set; }

    public Vector2 Value
    {
        get => Local + (_parent?.Value ?? Vector2.Zero);
        set => Local = value - (_parent?.Value ?? Vector2.Zero);
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
        
    public event PropertyChangedEventHandler? PropertyChanged;


    public override void Init(EditorWorld world, Entity entity)
    {
        _converter = world.PositionConverter;
        _parent = entity.GetComponent<ChildOf>()?.Parent?.GetRequiredComponent<Position>();

        if (_parent is not null)
        {
            _parent.PropertyChanged += Parent_OnPropertyChanged;
        }
    }

    private void Parent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
    }
}