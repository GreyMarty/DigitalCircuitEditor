using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public abstract class OnMouseDoubleClickBehavior : OnMouseButtonDownBehavior
{
    private DateTime? _lastClickTime;
    private bool _clicked;
    
    
    protected sealed override void OnMouseButtonDown(MouseButtonDown e)
    {
        var now = DateTime.Now;
        
        if (_clicked && now - _lastClickTime < TimeSpan.FromMilliseconds(300))
        {
            OnMouseDoubleClick(e);
            _clicked = false;
        }
        else
        {
            _clicked = true;
        }

        _lastClickTime = now;
    }
    
    protected abstract void OnMouseDoubleClick(MouseButtonDown e);
}