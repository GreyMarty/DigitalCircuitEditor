using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Behaviors.Filters;
using Editor.Core.Behaviors.Triggers;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Input;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories;

public class GhostNodeFactory : IEntityBuilderFactory
{
    public virtual IEntityBuilder Create()
    {
        var builder = Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<GhostNode>()
            .AddComponent(new CircleShape
            {
                Radius = 1
            })
            .AddComponent<Hoverable>()
            .AddComponent(new ChangeFillOnHover
            {
                HighlightColor = SKColors.LightGray
            })
            .AddComponent<DraggableConnectorSpawner>()
            .AddComponent(new CircleRenderer
            {
                Radius = 1,
                Fill = SKColor.Empty,
                Stroke = new SKColor(125, 125, 125, 125),
                StrokeThickness = 0.2f
            });

        builder
            .AddBehavior<SpawnBehavior, ITriggerArgs>(
                new MouseButtonDownTrigger()
                {
                    Button = MouseButton.Left,
                    Filters = [ new HoveredFilter() ]
                }
            )
            .AddBehavior<RequestRenderBehavior, ITriggerArgs>(
                new ComponentChangedTrigger<Position>(),
                new ComponentChangedTrigger<Renderer>()
            );

        return builder;
    }
}