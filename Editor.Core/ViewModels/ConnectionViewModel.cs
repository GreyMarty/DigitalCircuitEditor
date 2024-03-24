using System.ComponentModel;
using Editor.Component;
using Editor.Core.Components;

namespace Editor.Core.ViewModels;

public class ConnectionViewModel<TColor> : VisualElementViewModel<TColor>
{
    public Connection Connection { get; private set; } = default!;

    public Position? Source { get; private set; }
    public float SourceX => Source?.PixelsX ?? 0;
    public float SourceY => Source?.PixelsY ?? 0;
    
    public Position? Target { get; private set; }
    public float TargetOffsetX => Target?.PixelsX - SourceX ?? 0;
    public float TargetOffsetY => Target?.PixelsY - SourceY ?? 0;

    
    public override void Init(EditorWorld world, Entity entity)
    {
        Connection = entity.GetRequiredComponent<Connection>();
        Source = Source.Rebind(Connection.Source?.GetRequiredComponent<Position>(), Source_OnPropertyChanged);
        Target = Target.Rebind(Connection.Target?.GetRequiredComponent<Position>(), Target_OnPropertyChanged);
        
        Connection.PropertyChanged += Connection_OnPropertyChanged;
        
        base.Init(world, entity);
    }

    public override void Dispose()
    {
        Connection.PropertyChanged -= Connection_OnPropertyChanged;
        
        base.Dispose();
    }
    
    private void Connection_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Connection.Source):
                Source = Source.Rebind(Connection.Source?.GetRequiredComponent<Position>(), Source_OnPropertyChanged);
                break;
            case nameof(Connection.Target):
                Target = Target.Rebind(Connection.Target?.GetRequiredComponent<Position>(), Target_OnPropertyChanged);
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