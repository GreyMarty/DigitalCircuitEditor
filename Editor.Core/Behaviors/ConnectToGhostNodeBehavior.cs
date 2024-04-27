using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Prefabs.Factories;
using Editor.Core.Shapes;

namespace Editor.Core.Behaviors;

public class ConnectToGhostNodeBehavior : BehaviorBase<EditorContext, ITriggerArgs>
{
    private Position _positionComponent = default!;
    private Hoverable _hoverableComponent = default!;
    private Shape _shapeComponent = default!;
    private Node _nodeComponent = default!;
    

    public IEntityBuilderFactory ConnectionFactory { get; set; } = new ConnectionFactory();
    

    protected override void OnInit()
    {
        base.OnInit();

        _positionComponent = Entity.GetRequiredComponent<Position>()!;
        _hoverableComponent = Entity.GetRequiredComponent<Hoverable>()!;
        _shapeComponent = Entity.GetRequiredComponent<Shape>()!;
        _nodeComponent = Entity.GetRequiredComponent<Node>()!;
    }

    protected override void Perform(ITriggerArgs e)
    {
        foreach (var entity in Context.Entities)
        {
            var positionComponent = entity.GetComponent<Position>()?.Component;
            var childOfComponent = entity.GetComponent<ChildOf>()?.Component;
            var ghostNodeComponent = entity.GetComponent<GhostNode>()?.Component;
            
            if (positionComponent is null || childOfComponent?.Parent is null || entity.Active != true)
            {
                continue;
            }
            
            var relativePosition = positionComponent.Value - _positionComponent.Value;
            if (!_shapeComponent.Contains(relativePosition))
            {
                continue;
            }

            var parentNode = childOfComponent.Parent.GetRequiredComponent<BranchNode>().Component!;
            var connectionType = ghostNodeComponent.ConnectionType;

            parentNode.Connect(connectionType, _nodeComponent);
            break;
        }
    }
}