using System.ComponentModel;
using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.ViewModels;

public class ConnectionViewModel : EditorElementViewModel
{
    private ComponentRef<Connection> _connection = default!;
    private ComponentRef<Position>? _target;


    public float OffsetPixelsX => _target?.Component?.PixelsX - PixelsX ?? 0;
    public float OffsetPixelsY => _target?.Component?.PixelsY - PixelsY ?? 0;
    
    
    public override void Init(EditorWorld world, IEntity entity)
    {
        _connection = entity.GetRequiredComponent<Connection>();
        
        PropertyChanged += This_OnPropertyChanged;
        
        base.Init(world, entity);
        
        UpdateTarget();
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
        
        OnPropertyChanged(nameof(OffsetPixelsX));
        OnPropertyChanged(nameof(OffsetPixelsY));
    }
    
    private void This_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(PixelsX):
                OnPropertyChanged(nameof(OffsetPixelsX));
                break;
            case nameof(PixelsY):
                OnPropertyChanged(nameof(OffsetPixelsY));
                break;
        }
    }
    
    private void Connection_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        UpdateTarget();
    }

    private void Target_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(OffsetPixelsX));
        OnPropertyChanged(nameof(OffsetPixelsY));
    }
}