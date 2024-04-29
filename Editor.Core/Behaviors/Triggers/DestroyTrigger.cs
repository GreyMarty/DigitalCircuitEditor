using Editor.Component;

namespace Editor.Core.Behaviors.Triggers;

public class DestroyTrigger : TriggerBase<EditorContext, TriggerArgs>
{
    protected override void OnDestroy()
    {
        OnFired(TriggerArgs.Empty);
        base.OnDestroy();
    }
}