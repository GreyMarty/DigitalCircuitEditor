using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs;

public class GhostConnectionFactory<TConnection> : ConnectionFactory<TConnection> 
    where TConnection : Connection, new()
{
    public override IEntityBuilder Create()
    {
        return base
            .Create()
            .ConfigureComponent<ConnectionRenderer>(x =>
            {
                x.Stroke = new SKColor(125, 125, 125, 125);
            });
    }
}