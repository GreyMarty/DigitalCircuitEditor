using Editor.Component;

namespace Editor.Core.Behaviors.Filters;

public class MouseUnlockedFilter : TriggerFilterBase<EditorContext>
{
    public override bool CanFire() => !Context.MouseLocked;
}