using System.Numerics;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public abstract class LabeledShapeRenderer : ShapeRenderer
{
    protected SKFont Font { get; } = new SKFont(SKTypeface.FromFamilyName("Consolas"), 1);
    
    public float FontSize
    {
        get => Font.Size;
        set => Font.Size = value;
    }
    
    public Vector2 Anchor { get; set; }
    
    public string? Text { get; set; }
}