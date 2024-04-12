using Editor.Component;
using Editor.Core.Behaviors.Triggers.Args;
using Editor.Core.Events;
using Editor.Core.Input;

namespace Editor.Core.Behaviors.Triggers;

public class MouseMoveTrigger : TriggerBase<EditorContext, MouseMoveTriggerArgs>
{
    public MouseButton? Button { get; set; }
    
    
    protected override void OnInit()
    {
        Events.Subscribe<MouseMove>(e =>
        {
            if (Button is null || e.Button == Button)
            {
                OnFired(new MouseMoveTriggerArgs(
                    e.Button,
                    e.PositionConverter.ScreenToWorldSpace(e.OldPositionPixels),
                    e.PositionConverter.ScreenToWorldSpace(e.NewPositionPixels)
                ));
            }
        });
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<MouseMove>();
    }
}