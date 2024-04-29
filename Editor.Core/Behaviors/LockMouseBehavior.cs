using Editor.Component;

namespace Editor.Core.Behaviors;

public class LockMouseBehavior : BehaviorBase<EditorContext>
{
    protected override void Perform()
    {
        Context.MouseLocked = true;
    }
}

public class UnlockMouseBehavior : BehaviorBase<EditorContext>
{
    protected override void Perform()
    {
        Context.MouseLocked = false;
    }
}