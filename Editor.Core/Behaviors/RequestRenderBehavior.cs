using Editor.Component;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public class RequestRenderBehavior : BehaviorBase<EditorContext>
{
    protected override void Perform()
    {
        Context.EventBus.Publish(new RenderRequested(this));
    }
}