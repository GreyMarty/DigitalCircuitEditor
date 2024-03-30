using System.Windows;
using System.Windows.Controls;
using Editor.Component.Events;
using Editor.Core.Converters;
using Editor.Core.ViewModels;
using Editor.Core.Wpf.Events;

namespace Editor.Core.Wpf.Views;

public partial class EditorView : UserControl
{
    private MouseEventsRouter? _router;
    
    
    public EditorView()
    {
        DataContext = new EditorViewModel();
        InitializeComponent();
    }

    
    public EditorWorld World { get; private set; }
    

    private void EditorView_OnInitialized(object? sender, EventArgs e)
    {
        var unit = (float)(double)FindResource("Unit");
        World = new EditorWorld(UnitsToPixelsConverter.FromUnit(unit));

        var viewModel = (EditorViewModel)DataContext;
        DataContext = viewModel;
        
        World.EventBus.Subscribe<EntityInstantiated>(e =>
        {
            if (e.Entity.Components.FirstOrDefault(x => x is IViewComponent) is not IViewComponent component)
            {
                return;
            }
            
            Canvas.Children.Add(component.View);
        });
        
        World.EventBus.Subscribe<EntityDestroyed>(e =>
        {
            if (e.Entity.Components.FirstOrDefault(x => x is IViewComponent) is not IViewComponent component)
            {
                return;
            }
            
            Canvas.Children.Remove(component.View);
        });
        
        _router = new MouseEventsRouter(CanvasContainer, Canvas, World.EventBus, World.PositionConverter);
        _router.Bind();
        
        World.Init();
        viewModel.Init(World);
    }
}