using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.BinaryDiagrams;

namespace Editor.Core.Prefabs.BinaryDiagrams;

public class BinaryDiagramGhostNodeFactory : GhostNodeFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<GhostNode<BinaryDiagramConnectionType>>();
    }
}