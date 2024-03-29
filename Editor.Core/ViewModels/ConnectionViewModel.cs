using System.ComponentModel;
using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.ViewModels;

public class ConnectionViewModel : VisualElementViewModel
{
    private ComponentRef<Connection> _connection = default!;

    
    public ComponentRef<Position>? Source { get; private set; }
    public float SourceX => Source?.Component?.PixelsX ?? 0;
    public float SourceY => Source?.Component?.PixelsY ?? 0;
    
    public ComponentRef<Position>? Target { get; private set; }
    public float TargetOffsetX => Target?.Component?.PixelsX - SourceX ?? 0;
    public float TargetOffsetY => Target?.Component?.PixelsY - SourceY ?? 0;

    
    public override void Init(EditorWorld world, IEntity entity)
    {
        _connection = entity.GetRequiredComponent<Connection>();
        
        Source = Source.Rebind(_connection.Component?.Source?.GetRequiredComponent<Position>(), Source_OnPropertyChanged);
        Target = Target.Rebind(_connection.Component?.Target?.GetRequiredComponent<Position>(), Target_OnPropertyChanged);
        
        _connection.Component!.PropertyChanged += Connection_OnPropertyChanged;
        
        base.Init(world, entity);
    }

    public override void Dispose()
    {
        if (_connection.Component is not null)
        {
            _connection.Component.PropertyChanged -= Connection_OnPropertyChanged;
        }
        
        base.Dispose();
    }
    
    private void Connection_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_connection.Component.Source):
                Source = Source.Rebind(_connection.Component?.Source?.GetRequiredComponent<Position>(), Source_OnPropertyChanged);
                break;
            case nameof(_connection.Component.Target):
                Target = Target.Rebind(_connection.Component?.Target?.GetRequiredComponent<Position>(), Target_OnPropertyChanged);
                break;
        }
    }

    private void Source_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SourceX));
        OnPropertyChanged(nameof(SourceY));
        OnPropertyChanged(nameof(TargetOffsetX));
        OnPropertyChanged(nameof(TargetOffsetY));
    }
    
    private void Target_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(TargetOffsetX));
        OnPropertyChanged(nameof(TargetOffsetY));
    }
}