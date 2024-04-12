using Editor.Component;

namespace Editor.Core.Behaviors;

public class DestroyBehavior : BehaviorBase<EditorContext>
{
    protected override void Perform()
    {
        Context.Destroy(Entity);
    }
}