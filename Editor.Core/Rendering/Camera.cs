using System.ComponentModel;
using System.Numerics;
using Editor.Core.Events;
using Editor.Core.Input;
using SkiaSharp;
using TinyMessenger;

namespace Editor.Core.Rendering;

public class Camera : INotifyPropertyChanged, IDisposable
{
    private readonly ICameraTarget<SKImageInfo> _target;

    private bool _initialized;
    private ITinyMessengerHub _eventBus;
    private TinyMessageSubscriptionToken? _mouseMoveToken;
    private TinyMessageSubscriptionToken? _mouseWheelToken;
    
    
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
        _eventBus = context.EventBus;
        _mouseMoveToken = _eventBus.Subscribe<MouseMove>(OnMouseMove);
        _mouseWheelToken = _eventBus.Subscribe<MouseWheel>(OnMouseWheel);
        _initialized = true;
    }

    public void Dispose()
    {
        if (!_initialized)
        {
            return;
        }
        
        _eventBus.Unsubscribe<MouseMove>(_mouseMoveToken!);
        _eventBus.Unsubscribe<MouseWheel>(_mouseWheelToken!);
    }

    private void OnMouseMove(MouseMove e)
    {
        if (e.Button != MouseButton.Middle)
        {
            return;
        }
        
        var delta = e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels) - e.PositionConverter.ScreenToWorldSpace(e.OldPositionPixels);
        Position += delta * Scale;
        
        _eventBus.Publish(new RenderRequested(this));
    }
    
    private void OnMouseWheel(MouseWheel e)
    {
        var factor = e.Delta > 0 ? 1.1f : 1 / 1.1f;

        var mousePosition = e.PositionConverter.ScreenToCameraSpace(e.PositionPixels);
        var relativeMousePosition = (Position - mousePosition) / Scale;
        
        Scale = Math.Clamp(Scale * factor, 0.1f, 10f);
        Position = mousePosition + relativeMousePosition * Scale;
        
        _eventBus.Publish(new RenderRequested(this));
    }
}