using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components.IfDiagrams;

namespace Editor.Core.Prefabs.IfDiagrams;

public class IfDiagramNodeFactory : NodeFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<IfDiagramNode>()
            .AddComponent<ConnectToGhostNodeOnMouseButtonUp<IfDiagramConnection, IfDiagramConnectionType>>();
    }
}