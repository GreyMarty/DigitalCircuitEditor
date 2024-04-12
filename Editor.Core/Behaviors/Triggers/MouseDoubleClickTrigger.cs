using Editor.Component;
using Editor.Core.Behaviors.Triggers.Args;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Behaviors.Triggers;

public class MouseDoubleClickTrigger : TriggerBase<EditorContext, MouseButtonTriggerArgs>
{
    private DateTime? _lastClickTime;
    private bool _clicked;
    
    
    public MouseButton? Button { get; set; } = MouseButton.Left;
    
    
    protected override void OnInit()
    {
        Events.Subscribe<MouseButtonUp>(e =>
        {
            if (Button is not null && e.Button != Button)
            {
                return;
            }
            
            var now = DateTime.Now;

            if (_clicked && now - _lastClickTime < TimeSpan.FromMilliseconds(300))
            {
                OnFired(new MouseButtonTriggerArgs(e.Button, e.PositionConverter.ScreenToWorldSpace(e.PositionPixels)));
                _clicked = false;
            }
            else
            {
                _clicked = true;
            }

            _lastClickTime = now;
        });
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<MouseButtonDown>();
    }
}