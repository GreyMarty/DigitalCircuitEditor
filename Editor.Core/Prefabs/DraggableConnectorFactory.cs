using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;

namespace Editor.Core.Prefabs;

public class DraggableConnectorFactory<TConnectionType> : IEntityBuilderFactory 
    where TConnectionType : notnull
{
    public IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<DraggableConnector<TConnectionType>>()
            .AddComponent<RequestRenderOnComponentChange>();
    }
}