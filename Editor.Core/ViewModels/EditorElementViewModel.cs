using System.ComponentModel;
using System.Numerics;
using CommunityToolkit.Mvvm.Input;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Presentation;

namespace Editor.Core.ViewModels;

public partial class EditorElementViewModel : ViewModelBase
{
    private ComponentRef<Position>? _positionComponent;
    private ComponentRef<Hoverable>? _hoverableComponent;
    private ComponentRef<Selectable>? _selectableComponent;

    public Vector2 PixelsPosition => _positionComponent?.Component?.ValuePixels ?? Vector2.Zero;
    public float PixelsX => PixelsPosition.X;
    public float PixelsY => PixelsPosition.Y;

    public Color Stroke { get; protected set; } = Color.Primary;
    public Color Fill { get; protected set; } = Color.Secondary;


    public override void Init(EditorWorld world, IEntity entity)
    {
        _positionComponent = entity.GetComponent<Position>();
        _hoverableComponent = entity.GetComponent<Hoverable>();
        _selectableComponent = entity.GetComponent<Selectable>();

        if (_positionComponent?.Component is not null)
        {
            _positionComponent.Component.PropertyChanged += Position_OnPropertyChanged;
        }
        
        if (_hoverableComponent?.Component is not null)
        {
            _hoverableComponent.Component.PropertyChanged += HoverableOrSelectable_OnPropertyChanged;
        }
        
        if (_selectableComponent?.Component is not null)
        {
            _selectableComponent.Component.PropertyChanged += HoverableOrSelectable_OnPropertyChanged;
        }
    }

    public override void Dispose()
    {
        if (_hoverableComponent?.Component is not null)
        {
            _hoverableComponent.Component.PropertyChanged -= HoverableOrSelectable_OnPropertyChanged;
        }
        
        if (_selectableComponent?.Component is not null)
        {
            _selectableComponent.Component.PropertyChanged -= HoverableOrSelectable_OnPropertyChanged;
        }
        
        if (_positionComponent?.Component is not null)
        {
            _positionComponent.Component.PropertyChanged += Position_OnPropertyChanged;
        }
        
        base.Dispose();
    }

    protected virtual void EvaluateColors(bool hovered, bool selected)
    {
        if (selected)
        {
            Stroke = Color.PrimarySelected;
            Fill = Color.SecondarySelected;
            return;
        }

        if (hovered)
        {
            Stroke = Color.PrimaryHovered;
            Fill = Color.SecondaryHovered;
            return;
        }

        Stroke = Color.Primary;
        Fill = Color.Secondary;
    }
    
    [RelayCommand]
    private void OnHover(bool hovered)
    {
        if (_hoverableComponent?.Component is not null)
        {
            _hoverableComponent.Component.Hovered = hovered;
        }
    }

    private void Position_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(PixelsPosition));
        OnPropertyChanged(nameof(PixelsX));
        OnPropertyChanged(nameof(PixelsY));
    }
    
    private void HoverableOrSelectable_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        EvaluateColors(_hoverableComponent?.Component?.Hovered == true ,_selectableComponent?.Component?.Selected == true);
    }
}