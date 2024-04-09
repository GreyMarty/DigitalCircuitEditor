using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Components.BinaryDiagrams;
using Editor.Core.Spawners;

namespace Editor.Core.Prefabs.BinaryDiagrams;

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