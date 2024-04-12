using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Events;

namespace Editor.Core.Behaviors;

public class RequestPropertiesInspectorBehavior : BehaviorBase<EditorContext>
{
    protected override void Perform()
    {
        Context.EventBus.Publish(new RequestPropertiesInspector(Entity));
    }
}