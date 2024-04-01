using System.ComponentModel;
using System.Numerics;
using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.ViewModels;

public class ConnectionViewModel : EditorElementViewModel
{
    private ComponentRef<Connection> _connection = default!;
    private ComponentRef<Position>? _target;


    public string? Label => _connection.Component?.Label;

    public Vector2 OffsetPixels => _target?.Component?.ValuePixels - PixelsPosition ?? Vector2.Zero;
    public float OffsetPixelsX => OffsetPixels.X;
    public float OffsetPixelsY => OffsetPixels.Y;

    public float OffsetDistance => OffsetPixels.Length();
    public float OffsetRotation => MathF.Atan2(OffsetPixelsY, OffsetPixelsX) * (180 / MathF.PI);

    public float TextScale => MathF.Abs(OffsetRotation) < 90 ? 1 : -1;
    
    
    public override void Init(EditorWorld world, IEntity entity)
    {
        _connection = entity.GetRequiredComponent<Connection>();
        
        PropertyChanged += This_OnPropertyChanged;
        
        base.Init(world, entity);
        
        UpdateTarget();
        OnPropertyChanged(nameof(Label));
    }

    public override void Dispose()
    {
        
        if (_target?.Component is not null)
        {
            _target.Component.PropertyChanged -= Target_OnPropertyChanged;
        }

        if (_connection.Component is not null)
        {
            _connection.Component.PropertyChanged -= Connection_OnPropertyChanged;
        }
        
        base.Dispose();
    }

    private void UpdateTarget()
    {
        if (_target?.Component is not null)
        {
            _target.Component.PropertyChanged -= Target_OnPropertyChanged;
        }
        
        _target = _connection.Component?.Target?.GetComponent<Position>();
        
        if (_target?.Component is not null)
        {
            _target.Component.PropertyChanged += Target_OnPropertyChanged;
        }
        
        OnOffsetChanged();
    }

    private void OnOffsetChanged()
    {
        OnPropertyChanged(nameof(OffsetPixels));
        OnPropertyChanged(nameof(OffsetPixelsX));
        OnPropertyChanged(nameof(OffsetPixelsY));
        OnPropertyChanged(nameof(OffsetDistance));
        OnPropertyChanged(nameof(OffsetRotation));
        OnPropertyChanged(nameof(TextScale));
    }
    
    private void This_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(PixelsX) or nameof(PixelsY):
                OnOffsetChanged();
                break;
        }
    }
    
    private void Connection_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        UpdateTarget();
        OnPropertyChanged(nameof(Label));
    }

    private void Target_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnOffsetChanged();
    }
}