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
        _window = Application.Current.MainWindow;
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
    
// #region Test
//
//     
//     
//     private void TestOperation(IBooleanOperation operation)
//     {
//         var selected = Context.Entities
//             .Where(x => x.GetComponent<Selectable>()?.Component?.Selected == true)
//             .Select(x => x.GetComponent<Node>()?.Component)
//             .Where(x => x is not null)
//             .Take(2)
//             .ToList();
//
//         if (selected.Count != 2)
//         {
//             return;
//         }
//         
//         var diagramA = EntitiesToDiagramConverter.Convert(selected[0]!).Root.Reduce();
//         var diagramB = EntitiesToDiagramConverter.Convert(selected[1]!).Root.Reduce();
//
//         var diagramC = operation.Apply(diagramA, diagramB).Reduce();
//         
//         var builder = new InstantSpawnerFactory<BinaryDiagramSpawner>()
//             .Create()
//             .ConfigureComponent<Position>(x => x.Value = new Vector2(20, 0))
//             .ConfigureComponent<BinaryDiagramSpawner>(x =>
//             {
//                 x.Root = diagramC;
//                 x.Layout = new ForceDirectedLayout
//                 {
//                     Iterations = 1000
//                 };
//             })
//             .AddComponent<SpawnOnInit>();
//         Context.Instantiate(builder);
//     }
//     
// #endregion
    
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