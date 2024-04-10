using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Prefabs.Spawners;

namespace Editor.Core.Prefabs.Factories.BinaryDiagrams;

public class BinaryDiagramGhostNodeFactory : GhostNodeFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<GhostNode<BinaryDiagramConnectionType>>()
            .AddComponent<DraggableConnectorSpawner<BinaryDiagramConnection, BinaryDiagramConnectionType>>()
            .AddComponent<SpawnOnMouseButtonDown>();
    }
}