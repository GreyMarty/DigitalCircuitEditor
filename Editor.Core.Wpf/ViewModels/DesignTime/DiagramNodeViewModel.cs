using CommunityToolkit.Mvvm.Input;
using Editor.Core.Presentation;

namespace Editor.Core.Wpf.ViewModels.DesignTime;

public class DiagramNodeViewModel
{
    public float PixelsX { get; set; }
    public float PixelsY { get; set; }

    public Color Stroke { get; set; }
    public Color Fill { get; set; } = Color.Secondary;
    
    public RelayCommand<bool> HoverCommand { get; } = default!;
}