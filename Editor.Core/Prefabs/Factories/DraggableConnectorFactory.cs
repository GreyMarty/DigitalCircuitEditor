using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;

namespace Editor.Core.Prefabs.Factories;

public class DraggableConnectorFactory : IEntityBuilderFactory 
{
    public IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<DraggableConnector>()
            .AddComponent<RequestRenderOnComponentChange>();
    }
}