using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Behaviors;
using Editor.Core.Rendering.Behaviors;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using Editor.Core.Spawners;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public static class IfDiagramNodeSpawnerPrefab
{
    public static IEntityBuilder CreateBuilder(Vector2? position = null, IfDiagramConnectionType connectionType = IfDiagramConnectionType.Direct)
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
            .AddComponent(new HighlightOnHoverBehavior
            {
                HighlightColor = SKColors.LightGray
            })
            .AddComponent(new IfDiagramNodeSpawner
            {
                DestroyOnSpawn = true,
                ConnectionType = connectionType
            })
            .AddComponent<SpawnOnMouseButtonDownBehavior>()
            .AddComponent<RequestRenderBehavior>()
            .AddComponent(new CircleRenderer
            {
                Radius = 2,
                Fill = 0,
                StrokeThickness = 0.2f,
                Stroke = new SKColor(125, 125, 125, 125)
            });
    }
}