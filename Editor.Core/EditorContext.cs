using Editor.Component;
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;

namespace Editor.Core;

public class EditorContext : Context
{
    public Camera Camera { get; init; }
    public RendererCollection RenderingManager { get; init; } = new();

    public bool MouseLocked { get; set; }
    
    
    public override void Init()
    {
        base.Init();
        Camera.Init(this);
        RenderingManager.Init(this);
    }

    public override void Dispose()
    {
        Camera.Dispose();
        RenderingManager.Dispose();
        base.Dispose();
    }
}