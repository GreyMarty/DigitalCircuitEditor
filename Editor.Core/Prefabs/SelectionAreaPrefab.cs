using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Rendering.Adapters;
using Editor.Core.Rendering.Behaviors;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public static class SelectionAreaPrefab
{
    public static IEntityBuilder CreateBuilder(Vector2 position)
    {
        return Entity.CreateBuilder()
            .AddComponent(new Position()
            {
                Value = position
            })
            .AddComponent<RectangleShape>()
            .AddComponent<SelectionArea>()
            .AddComponent<RectangleShapeToRendererAdapter>()
            .AddComponent<RequestRenderBehavior>()
            .AddComponent(new RectangleRenderer
            {
                ZIndex = 100,
                Stroke = new SKColor(50, 125, 125, 50),
                StrokeThickness = 0.15f,
                Fill = new SKColor(100, 255, 255, 50)
            });
    }
}