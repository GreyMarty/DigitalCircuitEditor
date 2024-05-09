using Editor.Component;
using Editor.Core.Behaviors;

namespace Editor.Core.Prefabs.Factories.Circuits;

public class CircuitConnectionFactory : ConnectionFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .RemoveComponent<DestroyBehavior>();
    }
}