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

    protected Vector2 Position => _positionComponent.Value;
    

    protected override void OnInit(EditorContext context, IEntity entity)
    {
        _positionComponent = entity.GetRequiredComponent<Position>()!;
    }

    public abstract void Render(Camera camera, SKCanvas canvas);
}