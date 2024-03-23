﻿using System.ComponentModel;
using System.Numerics;
using Editor.Component;
using Editor.Core;

namespace Editor.ViewModel;

public class BddNodeViewModel : ViewModelComponentBase
{
    private IUnitsToPixelsConverter? _positionConverter;
    private Position? _positionComponent;
    
        
    public Vector2 Position => _positionComponent is null
        ? new Vector2()
        : _positionConverter?.Convert(_positionComponent.Value) ?? new Vector2();
    public float PositionX => Position.X;
    public float PositionY => Position.Y;
    
    public string? Label { get; set; }


    public override void Init(EditorWorld world, Entity entity)
    {
        _positionConverter = world.PositionConverter;
        
        _positionComponent = entity.GetRequiredComponent<Position>();
        _positionComponent.PropertyChanged += PositionComponent_OnPropertyChanged;
    }

    public override void Dispose()
    {
        if (_positionComponent is null)
        {
            return;
        }
        
        _positionComponent.PropertyChanged -= PositionComponent_OnPropertyChanged;
    }

    private void PositionComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_positionComponent.Value):
                OnPropertyChanged(nameof(Position));
                break;
        }
    }
}
