using Editor.Component;
using Editor.Core.Behaviors.Triggers.Args;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Behaviors.Triggers;

public class MouseButtonDownTrigger : TriggerBase<EditorContext, MouseButtonTriggerArgs>
{
    public MouseButton? Button { get; set; } = MouseButton.Left;
    
    
    protected override void OnInit()
    {
        Events.Subscribe<MouseButtonDown>(e =>
        {
            if (Button is null || e.Button == Button)
            {
                OnFired(new MouseButtonTriggerArgs(e.Button, e.PositionConverter.ScreenToWorldSpace(e.PositionPixels)));
            }
        });
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<MouseButtonDown>();
    }
}