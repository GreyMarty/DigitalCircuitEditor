using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public class IfDiagramConnectionFactory : IEntityBuilderFactory
{
    public IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<ChildOf>()
            .AddComponent<IfDiagramConnection>()
            .AddComponent(new ConnectionRenderer
            {
                Stroke = SKColors.Black,
                StrokeThickness = 0.2f
            });
    }
}