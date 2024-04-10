using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Behaviors;
using Editor.Core.Components.Diagrams.BinaryDiagrams;

namespace Editor.Core.Prefabs.Factories.BinaryDiagrams;

public class BinaryDiagramNodeFactory : NodeFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<BinaryDiagramNode>()
            .AddComponent<ConnectToGhostNodeOnMouseButtonUp<BinaryDiagramConnection, BinaryDiagramConnectionType>>()
            .AddComponent<NodeLabelToTextAdapter<BinaryDiagramConnectionType>>();
    }
}