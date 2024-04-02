using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public static class IfDiagramConnectionPrefab
{
    public static IEntityBuilder CreateBuilder(IEntity source, IEntity target, IfDiagramConnectionType type)
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new ChildOf
            {
                Parent = source
            })
            .AddComponent(new IfDiagramConnection
            {
                Type = type,
                Target = target
            })
            .AddComponent(new ConnectionRenderer
            {
                Stroke = SKColors.Black,
                StrokeThickness = 0.2f
            });
    }
}