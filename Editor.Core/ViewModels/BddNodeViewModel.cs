using System.ComponentModel;
using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Converters;

namespace Editor.Core.ViewModels;

public partial class BddNodeViewModel : ViewModelComponentBase
{
    private IUnitsToPixelsConverter _positionConverter = default!;
    private Position _positionComponent = default!;
    
    
    public Vector2 Position => _positionConverter.ToPixels(_positionComponent.Value);
    public float PositionX => Position.X;
    public float PositionY => Position.Y;
    
    
    public string? Label { get; set; }

    public Hoverable Hoverable { get; private set; } = new();

    public override void Init(EditorWorld world, Entity entity)
    {
        _positionConverter = world.PositionConverter;
        
        _positionComponent = entity.GetRequiredComponent<Position>();
        _positionComponent.PropertyChanged += PositionComponent_OnPropertyChanged;

        Hoverable = entity.GetRequiredComponent<Hoverable>();
    }

    public override void Dispose()
    {
        _positionComponent.PropertyChanged -= PositionComponent_OnPropertyChanged;
    }
    
    private void PositionComponent_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_positionComponent.Value):
                OnPropertyChanged(nameof(Position));
                OnPropertyChanged(nameof(PositionX));
                OnPropertyChanged(nameof(PositionY));
                break;
        }
    }
}
