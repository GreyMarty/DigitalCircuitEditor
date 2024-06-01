using Editor.Component;
using Editor.Core.Adapters;
using Editor.Core.Behaviors;
using Editor.Core.Behaviors.Filters;
using Editor.Core.Behaviors.Triggers;
using Editor.Core.Components.Diagrams.BinaryDiagrams;
using Editor.Core.Input;

namespace Editor.Core.Prefabs.Factories.BinaryDiagrams;

public class BinaryDiagramNodeFactory : NodeFactory
{
    public override IEntityBuilder Create()
    {
        return base.Create()
            .AddComponent<BinaryDiagramNode>()
            .AddBehavior<SelectDiagramBehavior, ITriggerArgs>(
                new MouseDoubleClickTrigger
                {
                    Button = MouseButton.Left,
                    Filters = [ new HoveredFilter() ]
                }
            );
    }
}