using Editor.Component;
using Editor.Core.Components.Circuits;
using Editor.Core.Rendering.Renderers.LogicGates;
using Editor.Core.Shapes;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Circuits;

public class MuxGateFactory : LogicGateFactoryBase
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .ConfigureComponent<RectangleShape>(x => x.Height *= 1.5f)
            .AddComponent<MuxGate>()
            .AddComponent(new MuxGateRenderer
            {
                Size = 3,
                Fill = SKColors.White,
                StrokeThickness = 0.2f,
                Stroke = SKColors.Black,
            });
    }
}