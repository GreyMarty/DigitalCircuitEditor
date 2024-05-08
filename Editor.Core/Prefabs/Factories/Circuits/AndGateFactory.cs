using Editor.Component;
using Editor.Core.Components.Circuits;
using Editor.Core.Rendering.Renderers.LogicGates;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Circuits;

public class AndGateFactory : LogicGateFactoryBase
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<AndGate>()
            .AddComponent(new AndGateRenderer
            {
                Size = 3,
                Fill = SKColors.White,
                StrokeThickness = 0.2f,
                Stroke = SKColors.Black,
            });
    }
}