using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public class DestroyOnMouseButtonUp : OnMouseButtonUpBehavior
{
    protected override void OnMouseButtonUp(MouseButtonUp e)
    {
        Context.Destroy(Entity);
    }
}