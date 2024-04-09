using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Components.BinaryDiagrams;
using Editor.Core.Components.IfDiagrams;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using Editor.Core.Spawners;
using SkiaSharp;

namespace Editor.Core.Prefabs.IfDiagrams;

public class IfDiagramGhostNodeFactory : GhostNodeFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<GhostNode<IfDiagramConnectionType>>();
    }
}