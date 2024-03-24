using System.ComponentModel;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Presentation;
using PropertyChanged;

namespace Editor.Core.ViewModels;

public class VisualElementViewModel<TColor> : ViewModelComponentBase
{
    public IPalette<TColor> Palette { get; set; } = default!;
    
    public Hoverable? Hoverable { get; private set; }
    public Selectable? Selectable { get; private set; }
    
    public float StrokeThickness { get; private set; }
    public TColor StrokeColor { get; private set; }
    public TColor Fill { get; private set; }


    public override void Init(EditorWorld world, Entity entity)
    {
        Hoverable = entity.GetComponent<Hoverable>();
        Selectable = entity.GetComponent<Selectable>();

        StrokeThickness = Palette.DefaultStokeThickness;
        StrokeColor = Palette.DefaultStrokeColor;
        Fill = Palette.DefaultFill;
        
        if (Hoverable is not null)
        {
            Hoverable.PropertyChanged += Dependency_OnPropertyChanged;
        }
        
        if (Selectable is not null)
        {
            Selectable.PropertyChanged += Dependency_OnPropertyChanged;
        }
    }

    public override void Dispose()
    {
        if (Hoverable is not null)
        {
            Hoverable.PropertyChanged -= Dependency_OnPropertyChanged;
        }
        
        if (Selectable is not null)
        {
            Selectable.PropertyChanged -= Dependency_OnPropertyChanged;
        }
    }

    private void Dependency_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (Selectable?.Selected == true)
        {
            StrokeThickness = Palette.SelectedStrokeThickness;
            StrokeColor = Palette.SelectedStrokeColor;
            Fill = Palette.SelectedFill;
            return;
        }

        if (Hoverable?.Hovered == true)
        {
            StrokeThickness = Palette.HoveredStrokeThickness;
            StrokeColor = Palette.HoveredStrokeColor;
            Fill = Palette.HoveredFill;
            return;
        }
        
        StrokeThickness = Palette.DefaultStokeThickness;
        StrokeColor = Palette.DefaultStrokeColor;
        Fill = Palette.DefaultFill;
    }
}