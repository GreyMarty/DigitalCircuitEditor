using System.ComponentModel;
using System.Numerics;
using Editor.Component.Events;
using Editor.Core.Events;
using Editor.Core.Input;
using SkiaSharp;
using TinyMessenger;

namespace Editor.Core.Rendering;

public class Camera : INotifyPropertyChanged, IDisposable
{
    private readonly ICameraTarget<SKImageInfo> _target;

    private bool _initialized;
    private IEventBusSubscriber _eventBus = default!;
    private EditorContext _context = default!;
    
    
    public Camera(ICameraTarget<SKImageInfo> target)
    {
        _target = target;
    }
    
    
    public Vector2 Position { get; set; }
    public float Scale { get; set; } = 1;

    public Vector2 Size => SizePixels / PixelsPerUnit;
    public Vector2 SizePixels => _target.Size;

    public float PixelsPerUnit => SizePixels.Y / 32;
    
    public SKColor Background = SKColors.White;
    
    
    public event PropertyChangedEventHandler? PropertyChanged;


    public void Init(EditorContext context)
    {
        _context = context;
        
        _eventBus = context.EventBus.Subscribe();
        _eventBus.Subscribe<MouseMove>(OnMouseMove);
        _eventBus.Subscribe<MouseWheel>(OnMouseWheel);
        _initialized = true;
    }

    public void Dispose()
    {
        if (!_initialized)
        {
            return;
        }
        
        _eventBus.Unsubscribe<MouseMove>();
        _eventBus.Unsubscribe<MouseWheel>();
    }

    private void OnMouseMove(MouseMove e)
    {
        if (e.Button != MouseButton.Middle)
        {
            return;
        }
        
        var delta = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels) - e.PositionConverter.ScreenToWorldSpace(e.OldPositionPixels);
        Position += delta * Scale;
        
        _context.EventBus.Publish(new RenderRequested(this));
    }
    
    private void OnMouseWheel(MouseWheel e)
    {
        var delta = e.Delta / 120f;
        var factor = MathF.Abs(delta) * (delta > 0 ? 1.1f : 1 / 1.1f);

        var mousePosition = e.PositionConverter.ScreenToCameraSpace(e.PositionPixels);
        var relativeMousePosition = (Position - mousePosition) / Scale;
        
        Scale = Math.Clamp(Scale * factor, 0.25f, 1f);
        Position = mousePosition + relativeMousePosition * Scale;
        
        _context.EventBus.Publish(new RenderRequested(this));
    }
}