using System.Numerics;
using CommunityToolkit.Mvvm.Input;
using Editor.Component;
using Editor.Component.Events;
using Editor.Core;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Converters;
using Editor.Core.Events;
using Editor.Core.Prefabs.Factories.Previews;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Prefabs.Spawners.Circuits;
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Serialization;
using Editor.DecisionDiagrams.Circuits;
using Editor.DecisionDiagrams.Extensions;
using Editor.DecisionDiagrams.Operations;
using Editor.ViewModel.Services;
using SkiaSharp;

namespace Editor.ViewModel;

public partial class EditorViewModel : ViewModel
{
    private readonly EditorSerializer _serializer = new();
    
    
    public EditorViewModel()
    {
        Menu = new EditorMenuViewModel
        {
            ApplyOperationCommand = ApplyOperationCommand,
            ReduceCommand = ReduceCommand,
            ConvertCommand = ConvertCommand,
            SaveCommand = SaveCommand,
            LoadCommand = LoadCommand
        };
    }


    public IFilePathPrompt FilePrompt { get; set; } = default!;
    public ICircuitPreviewService PreviewService { get; set; } = default!;
    
    public EditorContext? Context { get; private set; } = default!;
    public Action<Action>? Invoker { get; set; } = default!;
    
    
    public SKCameraTarget CameraTarget { get; } = new();
    public IPositionConverter PositionConverter { get; private set; } = default!;
    
    public IEventBusSubscriber EventBus { get; private set; } = default!;

    public EditorMenuViewModel Menu { get; }


    public event Action? Initialized;
    
    
    public void OnInitialized()
    {
        Context = new EditorContext
        {
            Camera = new Camera(CameraTarget),
            RenderingManager = new RendererCollection
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
        
        Initialized?.Invoke();
    }
    
    public void OnKeyDown(string key)
    {
        switch (key)
        {
            case "Delete":
                Context!.EventBus.Publish(new DestroyRequested(this));
                break;
        }
    }

    [RelayCommand]
    public void Reduce()
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
         
         Context.Instantiate(new BinaryDiagramPreviewFactory()
             .Create()
             .ConfigureComponent<Position>(x => x.Value = Vector2.One * 10000)
             .ConfigureComponent<BinaryDiagramSpawner>(x =>
             {
                 x.Root = diagram.Root;
             })
         );
    }

    [RelayCommand]
    public void ApplyOperation(IBooleanOperation operation)
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
         
         Context.Instantiate(new BinaryDiagramPreviewFactory()
             .Create()
             .ConfigureComponent<Position>(x => x.Value = Vector2.One * 10000)
             .ConfigureComponent<BinaryDiagramSpawner>(x =>
             {
                 x.Root = diagramC;
             })
         );
    }

    [RelayCommand]
    public void Convert()
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

        var circuit = diagram.Root.ToCircuit();
        
        PreviewService.Show(circuit);
    }

    [RelayCommand]
    public void Save()
    {
        if (FilePrompt.GetSaveFilePath() is not { } path)
        {
            return;
        }

        using var stream = File.CreateText(path);
        _serializer.Serialize(Context!, stream);
    }

    [RelayCommand]
    public void Load()
    {
        if (FilePrompt.GetOpenFilePath() is not { } path)
        {
            return;
        }

        using var stream = File.OpenText(path);
        _serializer.Deserialize(Context!, stream);
    }
}