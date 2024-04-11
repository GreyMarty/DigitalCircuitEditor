using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Wpf.Converters;
using Editor.Core.Wpf.Events;
using Editor.Core.Wpf.View.Inspector;
using Editor.Core.Wpf.ViewModel;
using SkiaSharp.Views.Desktop;

namespace Editor.Core.Wpf.View;

public partial class EditorView : UserControl
{
    private readonly SKCameraTarget _cameraTarget = new();
    private readonly IInspectorFactory _inspectorFactory = new InspectorFactory();
    private MouseEventsRouter _mouseEventsRouter;
    private IPositionConverter _positionConverter;

    private Window? _window;
    
    
    public EditorView()
    {
        InitializeComponent();
    }

    ~EditorView()
    {
        if (_window is not null)
        {
            _window.KeyDown -= Window_OnKeyDown;
        }
    }
    
    
    public EditorContext? Context { get; private set; }
    

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        _window = Application.Current.MainWindow;
        _window.KeyDown += Window_OnKeyDown;
        
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

        var eventBus = Context.EventBus.Subscribe();
        eventBus.Subscribe<RenderRequested>(_ => Canvas.InvalidateVisual());

        Context.Instantiate(Entity.CreateBuilder()
            .AddComponent<SelectionManager>()
        );
        
        Context.Init();
    }

    protected override void OnDrop(DragEventArgs e)
    {
        var viewModel = e.Data.GetData("Object") as EditorEntitiesListItemViewModel;

        if (viewModel is null)
        {
            return;
        }

        var position = _positionConverter.ScreenToWorldSpace(e.GetPosition(Canvas).ToVector2());
        var builder = viewModel.Factory.Create()
            .ConfigureComponent<Position>(x => x.Value = position)
            .AddComponent<SpawnOnInit>();
        Context?.Instantiate(builder);
        
        Context?.EventBus.Subscribe().Subscribe<RequestPropertiesInspector>(Context_OnPropertiesInspectorRequested);
        
        base.OnDrop(e);
    }

    private void Canvas_OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        _cameraTarget.Update(e.Info);
        Context?.Renderers.Render(Context.Camera, e.Surface.Canvas);
    }
    
    private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Popup.IsOpen = false;
    }
    
    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Delete:
                Context?.EventBus.Publish(new DestroyRequested(this));
                break;
        }
    }

    private void Popup_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
    }

    private void Context_OnPropertiesInspectorRequested(RequestPropertiesInspector e)
    {
        var control = _inspectorFactory.Create((IEntity)e.Sender);

        if (control is null)
        {
            return;
        }
        
        PopupContainer.Children.Clear();
        PopupContainer.Children.Add(control);
        Popup.IsOpen = true;
    }
}