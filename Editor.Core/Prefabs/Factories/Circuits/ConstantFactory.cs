using System.Numerics;
using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Components.Circuits;
using Editor.Core.Rendering.Renderers;
using SkiaSharp;

namespace Editor.Core.Prefabs.Factories.Circuits;

public class ConstantFactory : LogicGateFactoryBase
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<Constant>()
            .AddComponent<ConstantToTextAdapter>()
            .AddComponent(new LabeledRectangleRenderer
            {
                Width = 3,
                Height = 3,
                Fill = SKColors.White,
                StrokeThickness = 0.2f,
                Stroke = SKColors.Black,
                FontSize = 1.5f,
                Anchor = Vector2.One * 0.5f
            });
    }
}