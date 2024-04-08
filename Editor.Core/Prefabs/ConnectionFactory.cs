using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Rendering.Adapters;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public class ConnectionFactory<TConnection> : IEntityBuilderFactory
    where TConnection : Connection, new()
{
    public virtual IEntityBuilder Create()
    {
        return Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent(new ChildOf
            {
                DestroyWithParent = true
            })
            .AddComponent<TConnection>()
            .AddComponent<ConnectionLabelToTextAdapter>()
            .AddComponent(new ConnectionRenderer
            {
                Stroke = SKColors.Black,
                StrokeThickness = 0.2f,
                Anchor = new Vector2(0.5f, 0.5f)
            });
    }
}