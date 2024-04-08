using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Rendering.Adapters;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public class SelectionAreaFactory : IEntityBuilderFactory
{
    public IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<RectangleShape>()
            .AddComponent<SelectionArea>()
            .AddComponent<DestroyOnMouseButtonUp>()
            .AddComponent<RectShapeToRendererAdapter>()
            .AddComponent<RequestRenderOnComponentChange>()
            .AddComponent(new RectangleRenderer
            {
                ZIndex = 100,
                Stroke = new SKColor(50, 125, 125, 50),
                StrokeThickness = 0.15f,
                Fill = new SKColor(100, 255, 255, 50)
            });
    }
}