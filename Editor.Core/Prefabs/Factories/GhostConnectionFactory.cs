using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Behaviors;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Rendering.Effects;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories;

public class GhostConnectionFactory : ConnectionFactory 
{
    public override IEntityBuilder Create()
    {
        return base
            .Create()
            .RemoveComponent<Selectable>()
            .RemoveComponent<ChangeStrokeOnSelect>()
            .RemoveComponent<DestroyBehavior>()
            .RemoveComponent<CreateJointBehavior>()
            .RemoveComponent<ConnectionTypeToStrokeStyleAdapter>()
            .AddComponent<ConnectionLabelToTextAdapter>()
            .ConfigureComponent<ChildOf>(x =>
            {
                x.DestroyWithParent = true;
            })
            .ConfigureComponent<LabeledLineRenderer>(x =>
            {
                x.Stroke = new SKColor(0, 0, 0, 125);
            });
    }
}