using System.Windows;
using System.Windows.Controls;
using Editor.Core.Converters;
using Editor.Core.Events;
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Wpf.Converters;
using Editor.Core.Wpf.Events;
using Editor.Core.Wpf.ViewModel;
using SkiaSharp.Views.Desktop;

namespace Editor.Core.Wpf.View;

public partial class EditorView : UserControl
{
    private readonly SKCameraTarget _cameraTarget = new();
    private MouseEventsRouter _mouseEventsRouter;
    private IPositionConverter _positionConverter;
    
    
    public EditorView()
    {
        InitializeComponent();
    }
    
    
    public EditorContext? Context { get; private set; }
    

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        
        Context = new EditorContext
        {
            Camera = new Camera(_cameraTarget),
            Renderers = new RendererCollection
            {
                Invoker = x => Dispatcher.Invoke(x)
            }
        };

        _positionConverter = new PositionConverter(Context.Camera);
        _mouseEventsRouter = new MouseEventsRouter(Canvas, Context.EventBus, _positionConverter);
        _mouseEventsRouter.Bind();

        Context.EventBus.Subscribe<RenderRequested>(_ => Canvas.InvalidateVisual());
        
        Context.Init();
    }

    protected override void OnDrop(DragEventArgs e)
    {
        base.OnDrop(e);

        var viewModel = e.Data.GetData("Object") as EditorEntitiesListItemViewModel;

        if (viewModel is null)
        {
            return;
        }

        var position = _positionConverter.ScreenToWorldSpace(e.GetPosition(Canvas).ToVector2());
        Context?.Instantiate(viewModel.CreateBuilder(position));
    }

    private void Canvas_OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        _cameraTarget.Update(e.Info);
        Context?.Renderers.Render(Context.Camera, e.Surface.Canvas);
    }
}