using System.ComponentModel;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Presentation;
using PropertyChanged;

namespace Editor.Core.ViewModels;

public class VisualElementViewModel : ViewModelBase
{
    private ComponentRef<Hoverable>? _hoverableComponent;
    private ComponentRef<Selectable>? _selectableComponent;


    public StrokeStyle StrokeStyle { get; private set; } = StrokeStyle.Default;
    public FillStyle FillStyle { get; private set; } = FillStyle.Default;


    public override void Init(EditorWorld world, IEntity entity)
    {
        _hoverableComponent = entity.GetComponent<Hoverable>();
        _selectableComponent = entity.GetComponent<Selectable>();
        
        if (_hoverableComponent?.Component is not null)
        {
            _hoverableComponent.Component.PropertyChanged += Dependency_OnPropertyChanged;
        }
        
        if (_selectableComponent?.Component is not null)
        {
            _selectableComponent.Component.PropertyChanged += Dependency_OnPropertyChanged;
        }
    }

    public override void Dispose()
    {
        if (_hoverableComponent?.Component is not null)
        {
            _hoverableComponent.Component.PropertyChanged -= Dependency_OnPropertyChanged;
        }
        
        if (_selectableComponent?.Component is not null)
        {
            _selectableComponent.Component.PropertyChanged -= Dependency_OnPropertyChanged;
        }
        
        base.Dispose();
    }

    private void Dependency_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_selectableComponent?.Component?.Selected == true)
        {
            StrokeStyle = StrokeStyle.Selected;
            FillStyle = FillStyle.Selected;
            return;
        }

        if (_hoverableComponent?.Component?.Hovered == true)
        {
            StrokeStyle = StrokeStyle.Hovered;
            FillStyle = FillStyle.Hovered;
            return;
        }

        StrokeStyle = StrokeStyle.Default;
        FillStyle = FillStyle.Default;
    }
}