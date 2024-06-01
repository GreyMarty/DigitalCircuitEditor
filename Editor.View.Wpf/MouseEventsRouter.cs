using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Editor.Component.Events;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Rendering;
using Editor.View.Wpf.Converters;
using EditorMouseButton = Editor.Core.Input.MouseButton;

namespace Editor.View.Wpf;

public class MouseEventsRouter
{
    private readonly UIElement _source;
    private readonly IEventBus _target;
    private readonly IPositionConverter _converter;

    private bool _bound = false;
    
    private EditorMouseButton _button = 0;
    private Vector2 _position;
    
    
    public MouseEventsRouter(UIElement source, IEventBus target, IPositionConverter converter)
    {
        _source = source;
        _target = target;
        _converter = converter;
    }

    ~MouseEventsRouter()
    {
        Unbind();
    }

    
    public void Bind()
    {
        if (_bound)
        {
            return;
        }
        
        _source.MouseDown += SourceContainer_OnMouseButtonDown;
        _source.MouseUp += SourceContainer_OnMouseButtonUp;
        _source.MouseLeave += SourceContainer_OnMouseLeave;
        _source.Drop += SourceContainer_OnDrop;
        _source.MouseMove += SourceContainer_OnMouseMove;
        _source.MouseWheel += SourceContainer_OnMouseWheel;
    }

    public void Unbind()
    {
        if (!_bound)
        {
            return;
        }
        
        _source.MouseDown -= SourceContainer_OnMouseButtonDown;
        _source.MouseUp -= SourceContainer_OnMouseButtonUp;
        _source.MouseLeave -= SourceContainer_OnMouseLeave;
        _source.Drop -= SourceContainer_OnDrop;
        _source.MouseMove -= SourceContainer_OnMouseMove;
        _source.MouseWheel -= SourceContainer_OnMouseWheel;
    }
    
    private void SourceContainer_OnMouseButtonDown(object sender, MouseButtonEventArgs e)
    {
        _button = e.ChangedButton.ToEditor();
        _position = e.GetPosition(_source).ToVector2();
        
        _target.Publish(new MouseButtonDown(_source, _button, _position, _converter, (ModKeys)Keyboard.Modifiers));
    }
    
    private void SourceContainer_OnMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
        var button = e.ChangedButton.ToEditor();
        
        _button = 0;
        _position = e.GetPosition(_source).ToVector2();
        
        _target.Publish(new MouseButtonUp(_source, button, _position, _converter, (ModKeys)Keyboard.Modifiers));
    }
    
    private void SourceContainer_OnMouseLeave(object sender, MouseEventArgs e)
    {
        var button = _button;
        _button = 0;
        
        _target.Publish(new MouseButtonUp(_source, button, _position, _converter, (ModKeys)Keyboard.Modifiers));
    }
    
    private void SourceContainer_OnDrop(object sender, DragEventArgs e)
    {
        _button = 0;
        _position = e.GetPosition(_source).ToVector2();
        
        _target.Publish(new MouseButtonUp(_source, EditorMouseButton.Left, _position, _converter, (ModKeys)Keyboard.Modifiers));
    }

    private void SourceContainer_OnMouseMove(object sender, MouseEventArgs e)
    {
        _button = e switch
        {
            { LeftButton: MouseButtonState.Pressed } => EditorMouseButton.Left,
            { RightButton: MouseButtonState.Pressed } => EditorMouseButton.Right,
            { MiddleButton: MouseButtonState.Pressed } => EditorMouseButton.Middle,
            _ => 0
        };

        var oldPosition = _position;
        _position = e.GetPosition(_source).ToVector2();
        
        _target.Publish(new MouseMove(_source, oldPosition, _position, _converter, _button, (ModKeys)Keyboard.Modifiers));
    }
    
    private void SourceContainer_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        _position = e.GetPosition(_source).ToVector2();
        
        _target.Publish(new MouseWheel(_source, e.Delta, _position, _converter));
    }
}