using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Circuits;

public class CircuitPortFactory : IEntityBuilderFactory
{
    public IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new ChildOf
            {
                DestroyWithParent = true
            })
            .AddComponent(new CircleRenderer
            {
                Fill = SKColors.Black,
                Stroke = SKColors.Transparent,
                Radius = 0.1f
            });
    }
}