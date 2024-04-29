using Editor.Component;

namespace Editor.Core.Behaviors.Triggers;

public class InitTrigger : TriggerBase<EditorContext, TriggerArgs>
{
    protected override void OnInit()
    {
        base.OnInit();
        OnFired(TriggerArgs.Empty);
    }
}