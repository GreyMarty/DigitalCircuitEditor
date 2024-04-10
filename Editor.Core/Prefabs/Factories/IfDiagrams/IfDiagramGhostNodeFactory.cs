using Editor.Component;
using Editor.Core.Components.Diagrams;
using Editor.Core.Components.Diagrams.IfDiagrams;

namespace Editor.Core.Prefabs.Factories.IfDiagrams;

public class IfDiagramGhostNodeFactory : GhostNodeFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<GhostNode<IfDiagramConnectionType>>();
    }
}