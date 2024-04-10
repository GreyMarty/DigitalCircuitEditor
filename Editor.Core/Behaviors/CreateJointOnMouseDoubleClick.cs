using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Events;
using Editor.Core.Prefabs.Factories;

namespace Editor.Core.Behaviors;

public class CreateJointOnMouseDoubleClick : OnMouseDoubleClickBehavior
{
    private Hoverable _hoverableComponent = default!;
    private Connection _connectionComponent = default!;
    
    
    public IEntityBuilderFactory JointFactory = new ConnectionJointFactory();
    public IEntityBuilderFactory ConnectionFactory = new ConnectionFactory<Connection>();
    

    protected override void OnInit()
    {
        base.OnInit();

        _hoverableComponent = Entity.GetRequiredComponent<Hoverable>()!;
        _connectionComponent = Entity.GetRequiredComponent<Connection>()!;
    }

    protected override void OnMouseDoubleClick(MouseButtonDown e)
    {
        if (!_hoverableComponent.Hovered)
        {
            return;
        }
        
        var joint = Context.Instantiate(JointFactory.Create()
            .ConfigureComponent<Position>(x =>
            {
                x.Value = e.PositionConverter.ScreenToWorldSpace(e.PositionPixels);
            })
        );

        var jointComponent = joint.GetRequiredComponent<ConnectionJoint>().Component!;
        
        var connection = Context.Instantiate(ConnectionFactory.Create()
            .ConfigureComponent<ChildOf>(x =>
            {
                x.Parent = joint;
            })
            .ConfigureComponent<Connection>(x =>
            {
                x.Target = _connectionComponent.Target;
            })
        );
        
        _connectionComponent.Target = joint;

        jointComponent.Connection1 = _connectionComponent.Entity;
        jointComponent.Connection2 = connection;
    }
}