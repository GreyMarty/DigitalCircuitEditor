using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Behaviors;
using Editor.Core.Rendering.Behaviors;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public static class IfDiagramNodePrefab
{
    public static IEntityBuilder CreateBuilder(Vector2? position = null)
    {
        return Entity.CreateBuilder()
            .AddComponent(new Position
            {
                Value = position ?? Vector2.Zero
            })
            .AddComponent(new CircleShape
            {
                Radius = 2
            })
            .AddComponent<Hoverable>()
            .AddComponent<DragOnMouseMoveBehavior>()
            .AddComponent(new HighlightOnHoverBehavior
            {
                HighlightColor = SKColors.LightGray
            })
            .AddComponent<RequestRenderBehavior>()
            .AddComponent(new CircleRenderer
            {
                Radius = 2,
                Fill = SKColors.White,
                StrokeThickness = 0.2f,
                Stroke = SKColors.Black
            });
    }
}