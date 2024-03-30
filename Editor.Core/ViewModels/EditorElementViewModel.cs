using System.ComponentModel;
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

    public float PixelsX => _positionComponent?.Component?.PixelsX ?? 0;
    public float PixelsY => _positionComponent?.Component?.PixelsY ?? 0;

    public Color Stroke { get; private set; } = Color.Primary;
    public Color Fill { get; private set; } = Color.Secondary;


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
        OnPropertyChanged(nameof(PixelsX));
        OnPropertyChanged(nameof(PixelsY));
    }
    
    private void HoverableOrSelectable_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_selectableComponent?.Component?.Selected == true)
        {
            Stroke = Color.PrimarySelected;
            Fill = Color.SecondarySelected;
            return;
        }

        if (_hoverableComponent?.Component?.Hovered == true)
        {
            Stroke = Color.PrimaryHovered;
            Fill = Color.SecondaryHovered;
            return;
        }

        Stroke = Color.Primary;
        Fill = Color.Secondary;
    }
}