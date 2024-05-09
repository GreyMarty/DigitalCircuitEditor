using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Editor.Component;
using Editor.Core.Events;
using Editor.View.Wpf.Controls.Inspector;
using Editor.ViewModel;
using SkiaSharp.Views.Desktop;

namespace Editor.View.Wpf.Controls;

public partial class EditorView : UserControl
{
    private readonly InspectorFactory _inspectorFactory = new InspectorFactory();
    private MouseEventsRouter _mouseEventsRouter = default!;

    private bool _forceNextRedraw = false;
    
    private Window? _window;
    
    
    public EditorView()
    {
        DataContext = ViewModel;
        InitializeComponent();
    }

    ~EditorView()
    {
        if (_window is not null)
        {
            _window.KeyDown -= Window_OnKeyDown;
        }
    }


    public EditorViewModel ViewModel
    {
        get => (EditorViewModel)DataContext;
        set => DataContext = value;
    }
    

    private void This_OnLoaded(object sender, RoutedEventArgs e)
    {
        _window = Window.GetWindow(this);
        _window.KeyDown += Window_OnKeyDown;
        
        ViewModel.OnInitialized();
        
        ViewModel.EventBus.Subscribe<RenderRequested>(e =>
        {
            _forceNextRedraw |= e.Force;
            Canvas.InvalidateVisual();
        });
        ViewModel.EventBus.Subscribe<RequestPropertiesInspector>(Context_OnPropertiesInspectorRequested);
        
        _mouseEventsRouter = new MouseEventsRouter(Canvas, ViewModel.Context.EventBus, ViewModel.PositionConverter);
        _mouseEventsRouter.Bind();

        Canvas.InvalidateVisual();
    }
    
    private void Canvas_OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        if (ViewModel.Context is null)
        {
            return;
        }
        
        ViewModel.CameraTarget.Update(e.Info);
        ViewModel.Context.RenderingManager.Render(ViewModel.Context.Camera, e.Surface.Canvas, _forceNextRedraw);
        _forceNextRedraw = false;
    }
    
    private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Popup.IsOpen = false;
    }
    
    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        ViewModel.OnKeyDown(e.Key.ToString());
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