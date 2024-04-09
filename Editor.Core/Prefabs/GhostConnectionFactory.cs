using Editor.Component;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Rendering.Effects;
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
            .RemoveComponent<Selectable>()
            .RemoveComponent<ChangeStrokeOnSelect>()
            .RemoveComponent<DestroyOnMouseButtonDown>()
            .ConfigureComponent<ChildOf>(x =>
            {
                x.DestroyWithParent = true;
            })
            .ConfigureComponent<LabeledLineRenderer>(x =>
            {
                x.Stroke = new SKColor(125, 125, 125, 125);
            });
    }
}