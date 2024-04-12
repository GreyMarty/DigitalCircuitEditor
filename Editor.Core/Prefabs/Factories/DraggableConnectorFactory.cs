using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Behaviors.Triggers;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;

namespace Editor.Core.Prefabs.Factories;

public class DraggableConnectorFactory : IEntityBuilderFactory 
{
    public IEntityBuilder Create()
    {
        var builder = Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<DraggableConnector>();

        builder.AddBehavior<RequestRenderBehavior, ITriggerArgs>(
            new ComponentChangedTrigger<Position>()
        );

        return builder;
    }
}