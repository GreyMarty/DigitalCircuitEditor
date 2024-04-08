using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using SkiaSharp;

namespace Editor.Core.Rendering.Renderers;

public abstract class Renderer : EditorComponentBase
{
    private Position _positionComponent = default!;
    
    public RenderLayer Layer { get; init; }
    public int ZIndex { get; init; } = 0;

    public bool Visible { get; set; } = true;
    
    public Vector2 Position => _positionComponent.Value;
    

    protected override void OnInit()
    {
        _positionComponent = Entity.GetRequiredComponent<Position>()!;
    }

    public void Render(Camera camera, SKCanvas canvas)
    {
        if (!Visible)
        {
            return;
        }
        
        canvas.Save();
        
        canvas.Translate(Position.X, Position.Y);
        OnRender(camera, canvas);
        
        canvas.Restore();
    }

    protected abstract void OnRender(Camera camera, SKCanvas canvas);
}