using System.Numerics;
using System.Text;
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
using Editor.Core.Rendering;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Serialization;
using Editor.DecisionDiagrams;
using Editor.DecisionDiagrams.Circuits;
using Editor.DecisionDiagrams.Extensions;
using Editor.DecisionDiagrams.Operations;
using Editor.ViewModel.Services;
using SkiaSharp;

namespace Editor.ViewModel;

public partial class EditorViewModel : ViewModel
{
    private readonly EditorSerializer _serializer = new();
    private string? _saveFilePath;
    
    
    public EditorViewModel()
    {
        Menu = new EditorMenuViewModel
        {
            ApplyOperationCommand = ApplyOperationCommand,
            ReduceCommand = ReduceCommand,
            ConvertCommand = ConvertCommand,
            SaveCommand = SaveCommand,
            SaveAsCommand = SaveAsCommand,
            LoadCommand = LoadCommand,
            CopyCommand = CopyCommand,
            PasteCommand = PasteCommand,
            DuplicateCommand = DuplicateCommand
        };
    }


    public IFilePathPrompt FilePrompt { get; set; } = default!;
    public ICircuitPreviewService PreviewService { get; set; } = default!;
    public IClipboardService ClipboardService { get; set; } = default!;
    
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

         try
         {
             var diagram = EntitiesToDiagramConverter.Convert(rootEntity.GetRequiredComponent<Node>()!);
             diagram.Root = diagram.Root.Reduce();

             Context.Instantiate(new BinaryDiagramPreviewFactory()
                 .Create()
                 .ConfigureComponent<Position>(x => x.Value = Vector2.One * 10000)
                 .ConfigureComponent<BinaryDiagramSpawner>(x => { x.Root = diagram.Root; })
             );
         }
         catch
         {
             // Ignore
         }
         
    }

    [RelayCommand]
    public void ApplyOperation(IOperation operation)
    {
        var selected = Context.Entities
            .Where(x => x.GetComponent<Selectable>()?.Component?.Selected == true)
            .Select(x => x.GetComponent<Node>()?.Component)
            .Where(x => x is not null)
            .Take(2)
            .ToList();

        var diagramC = default(INode);

        try
        {
            switch (operation)
            {
                case IUnaryOperation:
                {
                    if (selected.Count != 1)
                    {
                        return;
                    }

                    var diagramA = EntitiesToDiagramConverter.Convert(selected[0]!).Root.Reduce();
                    diagramC = operation.Apply(diagramA);
                    break;
                }

                case IBinaryOperation:
                {
                    if (selected.Count != 2)
                    {
                        return;
                    }

                    var diagramA = EntitiesToDiagramConverter.Convert(selected[0]!).Root.Reduce();
                    var diagramB = EntitiesToDiagramConverter.Convert(selected[1]!).Root.Reduce();
                    diagramC = operation.Apply(diagramA, diagramB).Reduce();
                    break;
                }

                default:
                    return;
            }

            Context.Instantiate(new BinaryDiagramPreviewFactory()
                .Create()
                .ConfigureComponent<Position>(x => x.Value = Vector2.One * 10000)
                .ConfigureComponent<BinaryDiagramSpawner>(x => { x.Root = diagramC; })
            );
        }
        catch
        {
            // Ignore
        }
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

        try
        {
            var diagram = EntitiesToDiagramConverter.Convert(rootEntity.GetRequiredComponent<Node>()!);
            diagram.Root = diagram.Root.Reduce();

            var circuit = diagram.Root.ToCircuit();

            PreviewService.Show(circuit);
        }
        catch
        {
            // Ignore
        }
    }

    [RelayCommand]
    public void Save()
    {
        if (_saveFilePath is null)
        {
            SaveAs();
            return;
        }

        try
        {
            using var stream = File.CreateText(_saveFilePath);
            _serializer.Serialize(stream, Context!);
        }
        catch
        {
            // Ignore
        }
    }
    
    [RelayCommand]
    public void SaveAs()
    {
        if (FilePrompt.GetSaveFilePath(_saveFilePath) is not { } path)
        {
            return;
        }

        _saveFilePath = path;
        Menu.FileName = Path.GetFileName(_saveFilePath);

        try
        {
            using var stream = File.CreateText(_saveFilePath);
            _serializer.Serialize(stream, Context!);
        }
        catch
        {
            // Ignore
        }
    }

    [RelayCommand]
    public void Load()
    {
        if (FilePrompt.GetOpenFilePath() is not { } path)
        {
            return;
        }

        _saveFilePath = path;
        Menu.FileName = Path.GetFileName(_saveFilePath);

        try
        {
            using var stream = File.OpenText(_saveFilePath);
            _serializer.Deserialize(stream, Context!, true, deserializeCamera: true);
        }
        catch
        {
            // Ignore
        }
    }

    [RelayCommand]
    public void Copy()
    {
        var entities = Context.Entities
            .Where(x => x.GetComponent<Selectable>()?.Component?.Selected == true)
            .ToList();

        if (entities.Count == 0)
        {
            return;
        }

        try
        {
            using var writer = new StringWriter();
            _serializer.Serialize(writer, entities);
            ClipboardService.Copy(writer.ToString());
        }
        catch
        {
            // Ignore
        }
    }

    [RelayCommand]
    public void Paste()
    {
        if (ClipboardService.Paste() is not { } clipboardText)
        {
            return;
        }

        var oldSelectables = Context.Entities
            .Select(x => x.GetComponent<Selectable>()?.Component)
            .Where(x => x is not null)
            .ToList();

        try
        {
            using var reader = new StringReader(clipboardText);
            _serializer.Deserialize(reader, Context, false, true, deserializeCamera: false, new Vector2(5, 5));

            foreach (var selectable in oldSelectables)
            {
                selectable!.Selected = false;
            }
        }
        catch
        {
            // ignored
        }
    }

    [RelayCommand]
    public void Duplicate()
    {
        Copy();
        Paste();
    }
}