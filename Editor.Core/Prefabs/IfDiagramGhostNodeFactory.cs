using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public class IfDiagramGhostNodeFactory : IEntityBuilderFactory
{
    public IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<IfDiagramGhostNode>()
            .AddComponent(new CircleShape
            {
                Radius = 1
            })
            .AddComponent<Hoverable>()
            .AddComponent(new ChangeFillOnHover
            {
                HighlightColor = SKColors.LightGray
            })
            .AddComponent<RequestRenderOnComponentChange>()
            .AddComponent(new CircleRenderer
            {
                Radius = 1,
                Fill = SKColor.Empty,
                Stroke = new SKColor(125, 125, 125, 125),
                StrokeThickness = 0.2f
            });
    }
}