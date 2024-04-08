using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components.BinaryDiagrams;
using Editor.Core.Rendering.Adapters;

namespace Editor.Core.Prefabs.BinaryDiagrams;

public class BinaryDiagramNodeFactory : NodeFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<BinaryDiagramNode>()
            .AddComponent<ConnectToGhostNodeOnMouseButtonUp<BinaryDiagramConnection, BinaryDiagramConnectionType>>()
            .AddComponent<BinaryDiagramNodeVariableIdToTextAdapter>();
    }
}