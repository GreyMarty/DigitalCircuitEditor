namespace Editor.Core.Presentation;

public interface IPalette<TColor>
{
    public float DefaultStokeThickness { get; }
    public TColor DefaultStrokeColor { get; }
    public float HoveredStrokeThickness { get; }
    public TColor HoveredStrokeColor { get; }
    public float SelectedStrokeThickness { get; }
    public TColor SelectedStrokeColor { get; }
    
    public TColor DefaultFill { get; }
    public TColor HoveredFill { get; }
    public TColor SelectedFill { get; }
}