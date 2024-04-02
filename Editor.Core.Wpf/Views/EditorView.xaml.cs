using System.Windows;
using System.Windows.Controls;
using Editor.Core.Events;
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Wpf.Events;
using SkiaSharp.Views.Desktop;

namespace Editor.Core.Wpf.Views;

public partial class EditorView : UserControl
{
    private readonly SKCameraTarget _cameraTarget = new();
    private MouseEventsRouter? _mouseEventsRouter;
    
    
    public EditorView()
    {
        InitializeComponent();
    }
    
    
    public EditorContext? Context { get; private set; }
    
    
    private void EditorView_OnInitialized(object? sender, EventArgs e)
    {
        Context = new EditorContext
        {
            Camera = new Camera(_cameraTarget),
            Renderers = new RendererCollection
            {
                Invoker = x => Dispatcher.Invoke(x)
            }
        };

        _mouseEventsRouter = new MouseEventsRouter(Canvas, Context.EventBus, new PositionConverter(Context.Camera));
        _mouseEventsRouter.Bind();

        Context.EventBus.Subscribe<RenderRequested>(_ => Canvas.InvalidateVisual());
        
        Context.Init();
    }
    
    private void Canvas_OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        _cameraTarget.Update(e.Info);
        Context?.Renderers.Render(Context.Camera, e.Surface.Canvas);
    }
}