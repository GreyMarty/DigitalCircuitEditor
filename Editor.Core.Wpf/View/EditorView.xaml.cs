using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Converters;
using Editor.Core.Events;
using Editor.Core.Layout;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Wpf.Converters;
using Editor.Core.Wpf.Events;
using Editor.Core.Wpf.View.Inspector;
using Editor.Core.Wpf.ViewModel;
using Editor.DecisionDiagrams;
using Editor.DecisionDiagrams.Extensions;
using Editor.DecisionDiagrams.Operations;
using SkiaSharp.Views.Desktop;
using BranchNode = Editor.DecisionDiagrams.BranchNode;

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
        eventBus.Subscribe<RequestPropertiesInspector>(Context_OnPropertiesInspectorRequested);
        
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
        
        base.OnDrop(e);
    }

#region Test

    private void TestReduce()
    {
        var rootEntity = Context.Entities
            .Where(x => x.GetComponent<Selectable>()?.Component?.Selected == true)
            .FirstOrDefault(x => x.GetComponent<Node>() is not null);

        if (rootEntity is null)
        {
            return;
        }
        
        var diagram = EntitiesToDiagramConverter.Convert(rootEntity.GetRequiredComponent<Node>()!);
        diagram.Root = diagram.Root.Reduce();
        
        var builder = new InstantSpawnerFactory<BinaryDiagramSpawner>()
            .Create()
            .ConfigureComponent<Position>(x => x.Value = new Vector2(20, 0))
            .ConfigureComponent<BinaryDiagramSpawner>(x =>
            {
                x.Root = diagram.Root;
                x.Layout = new ForceDirectedLayout
                {
                    Iterations = 1000
                };
            })
            .AddComponent<SpawnOnInit>();
        Context.Instantiate(builder);
    }
    
    private void TestOperation(IBooleanOperation operation)
    {
        var selected = Context.Entities
            .Where(x => x.GetComponent<Selectable>()?.Component?.Selected == true)
            .Select(x => x.GetComponent<Node>()?.Component)
            .Where(x => x is not null)
            .Take(2)
            .ToList();

        if (selected.Count != 2)
        {
            return;
        }
        
        var diagramA = EntitiesToDiagramConverter.Convert(selected[0]!).Root.Reduce();
        var diagramB = EntitiesToDiagramConverter.Convert(selected[1]!).Root.Reduce();

        var diagramC = operation.Apply(diagramA, diagramB).Reduce();
        
        var builder = new InstantSpawnerFactory<BinaryDiagramSpawner>()
            .Create()
            .ConfigureComponent<Position>(x => x.Value = new Vector2(20, 0))
            .ConfigureComponent<BinaryDiagramSpawner>(x =>
            {
                x.Root = diagramC;
                x.Layout = new ForceDirectedLayout
                {
                    Iterations = 1000
                };
            })
            .AddComponent<SpawnOnInit>();
        Context.Instantiate(builder);
    }
    
#endregion
    
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
            
            case Key.C:
                TestReduce();
                break;
            
            case Key.A:
                TestOperation(new And());
                break;
            
            case Key.O:
                TestOperation(new Or());
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