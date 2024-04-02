using Editor.Component;
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core;

public class EditorContext : World
{
    public Camera Camera { get; init; }
    public RendererCollection Renderers { get; init; } = new();

    
    public override void Init()
    {
        base.Init();
        Camera.Init(this);
        Renderers.Init(this);
    }

    public override void Dispose()
    {
        Camera.Dispose();
        Renderers.Dispose();
        base.Dispose();
    }
}