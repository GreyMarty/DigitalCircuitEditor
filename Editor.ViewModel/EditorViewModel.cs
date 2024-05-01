using System.ComponentModel.DataAnnotations;
using System.Numerics;
using CommunityToolkit.Mvvm.Input;
using Editor.Component;
using Editor.Component.Events;
using Editor.Core;
using Editor.Core.Components;
using Editor.Core.Events;
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.ViewModel;

public partial class EditorViewModel : ViewModel
{
    public EditorContext Context { get; private set; } = default!;
    public Action<Action>? Invoker { get; set; } = default!;

    public ToolBarViewModel ToolBar { get; init; } = default!;
    
    public SKCameraTarget CameraTarget { get; } = new();
    public IPositionConverter PositionConverter { get; private set; } = default!;
    
    public IEventBusSubscriber EventBus { get; private set; } = default!;
    
    
    public void OnInitialized()
    {
        Context = new EditorContext
        {
            Camera = new Camera(CameraTarget),
            Renderers = new RendererCollection
            {
                Invoker = Invoker ?? (x => x.Invoke())
            }
        };
        
        PositionConverter = new PositionConverter(Context.Camera);
        EventBus = Context.EventBus.Subscribe();
        
        Context.Instantiate(Entity.CreateBuilder()
            .AddComponent<SelectionManager>()
        );
        
        Context.Init();
        
        Context.Instantiate(Entity.CreateBuilder()
            .AddComponent(new GridRenderer
            {
                MajorStep = 4,
                Subdivisions = 5,
                Stroke = SKColors.Gray,
                StrokeThickness = 0.25f,
                Layer = RenderLayer.PreRender
            })
        );
    }
    
    public void OnKeyDown(string key)
    {
        switch (key)
        {
            case "Delete":
                Context.EventBus.Publish(new DestroyRequested(this));
                break;
        }
    }
    
    public void OnToolBarItemSelected(ToolBarViewModel sender, ToolBarItemViewModel? item)
    {
        if (item?.Factory is not { } factory)
        {
            return;
        }

        Context.Instantiate(factory.Create()
            .ConfigureComponent<Position>(x => x.Value = Vector2.One * 10000)
        );
    }
}