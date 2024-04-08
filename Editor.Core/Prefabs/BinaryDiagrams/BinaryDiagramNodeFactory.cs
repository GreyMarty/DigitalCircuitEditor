using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Behaviors;
using Editor.Core.Components.BinaryDiagrams;

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