using System.Numerics;
using System.Windows;
using System.Windows.Input;
using Editor.Core.Converters;
using Editor.Core.Events;
using Editor.Core.Wpf.Converters;
using TinyMessenger;
using EditorMouseButton = Editor.Core.Input.MouseButton;

namespace Editor.Core.Wpf.Events;

public class MouseEventsRouter
{
    private readonly UIElement _sourceContainer;
    private readonly UIElement _source;
    private readonly ITinyMessengerHub _target;
    private readonly IUnitsToPixelsConverter _converter;

    private bool _bound = false;
    
    private EditorMouseButton _button = 0;
    private Vector2? _sourcePosition;
    private Vector2 _position;
    
    
    public MouseEventsRouter(UIElement sourceContainer, UIElement source, ITinyMessengerHub target, IUnitsToPixelsConverter converter)
    {
        _sourceContainer = sourceContainer;
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
        
        _sourceContainer.MouseDown += SourceContainer_OnMouseButtonDown;
        _sourceContainer.MouseUp += SourceContainer_OnMouseButtonUp;
        _sourceContainer.MouseMove += SourceContainer_OnMouseMove;
        _sourceContainer.MouseWheel += SourceContainer_OnMouseWheel;
    }

    public void Unbind()
    {
        if (!_bound)
        {
            return;
        }
        
        _sourceContainer.MouseDown -= SourceContainer_OnMouseButtonDown;
        _sourceContainer.MouseUp -= SourceContainer_OnMouseButtonUp;
        _sourceContainer.MouseMove -= SourceContainer_OnMouseMove;
        _sourceContainer.MouseWheel -= SourceContainer_OnMouseWheel;
    }
    
    private void SourceContainer_OnMouseButtonDown(object sender, MouseButtonEventArgs e)
    {
        _button = e.ChangedButton.ToEditor();
        _position = _converter.ToUnits(e.GetPosition(_source).ToVector2());
        
        _target.Publish(new MouseButtonDown(_source, _button, _position));
    }
    
    private void SourceContainer_OnMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
        var button = e.ChangedButton.ToEditor();
        
        _button = 0;
        _position = _converter.ToUnits(e.GetPosition(_source).ToVector2());
        
        _target.Publish(new MouseButtonUp(_source, button, _position));
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
        _position = _converter.ToUnits(e.GetPosition(_source).ToVector2());

        var newSourcePosition = _source.TranslatePoint(new Point(), _sourceContainer).ToVector2();
        
        if (_sourcePosition is not null && _sourcePosition != newSourcePosition)
        {
            oldPosition += _converter.ToUnits(_sourcePosition.Value - newSourcePosition);
        }
        
        _sourcePosition = newSourcePosition;
        
        _target.Publish(new MouseMove(_source, oldPosition, _position, _button));
    }
    
    private void SourceContainer_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        _position = _converter.ToUnits(e.GetPosition(_source).ToVector2());
        
        _target.Publish(new MouseWheel(_source, e.Delta, _position));
    }
}