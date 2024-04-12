using Editor.Component;

namespace Editor.Core.Behaviors.Triggers;

public class InitTrigger : TriggerBase<EditorContext, TriggerArgs>
{
    protected override void OnInit()
    {
        OnFired(TriggerArgs.Empty);
    }
}