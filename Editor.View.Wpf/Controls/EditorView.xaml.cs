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
    private readonly EditorViewModel _viewModel = new();
    private readonly IInspectorFactory _inspectorFactory = new InspectorFactory();
    private MouseEventsRouter _mouseEventsRouter;

    private Window? _window;
    
    
    public EditorView()
    {
        DataContext = _viewModel;
        InitializeComponent();
    }

    ~EditorView()
    {
        if (_window is not null)
        {
            _window.KeyDown -= Window_OnKeyDown;
        }
    }
    
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        _window = Application.Current.MainWindow;
        _window.KeyDown += Window_OnKeyDown;
        
        _viewModel.OnInitialized();
        
        _viewModel.EventBus.Subscribe<RenderRequested>(_ => Canvas.InvalidateVisual());
        _viewModel.EventBus.Subscribe<RequestPropertiesInspector>(Context_OnPropertiesInspectorRequested);
        
        _mouseEventsRouter = new MouseEventsRouter(Canvas, _viewModel.Context.EventBus, _viewModel.PositionConverter);
        _mouseEventsRouter.Bind();
    }

// #region Test
//
//     private void TestReduce()
//     {
//         var rootEntity = Context.Entities
//             .Where(x => x.GetComponent<Selectable>()?.Component?.Selected == true)
//             .FirstOrDefault(x => x.GetComponent<Node>() is not null);
//
//         if (rootEntity is null)
//         {
//             return;
//         }
//         
//         var diagram = EntitiesToDiagramConverter.Convert(rootEntity.GetRequiredComponent<Node>()!);
//         diagram.Root = diagram.Root.Reduce();
//         
//         var builder = new InstantSpawnerFactory<BinaryDiagramSpawner>()
//             .Create()
//             .ConfigureComponent<Position>(x => x.Value = new Vector2(20, 0))
//             .ConfigureComponent<BinaryDiagramSpawner>(x =>
//             {
//                 x.Root = diagram.Root;
//                 x.Layout = new ForceDirectedLayout
//                 {
//                     Iterations = 1000
//                 };
//             })
//             .AddComponent<SpawnOnInit>();
//         Context.Instantiate(builder);
//     }
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
        _viewModel.CameraTarget.Update(e.Info);
        _viewModel.Context.Renderers.Render(_viewModel.Context.Camera, e.Surface.Canvas);
    }
    
    private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Popup.IsOpen = false;
    }
    
    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        _viewModel.OnKeyDown(e.Key.ToString());
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