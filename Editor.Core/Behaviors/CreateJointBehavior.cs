using Editor.Component;
using Editor.Core.Behaviors.Triggers.Args;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;
using Editor.Core.Prefabs.Factories;

namespace Editor.Core.Behaviors;

public class CreateJointBehavior : BehaviorBase<EditorContext, IPositionArgs>
{
    private Connection _connectionComponent = default!;
    
    
    public IEntityBuilderFactory JointFactory = new ConnectionJointFactory();
    public IEntityBuilderFactory ConnectionFactory = new ConnectionFactory();
    

    protected override void OnInit()
    {
        base.OnInit();
        
        _connectionComponent = Entity.GetRequiredComponent<Connection>()!;
    }

    protected override void Perform(IPositionArgs e)
    {
        var joint = Context.Instantiate(JointFactory.Create()
            .ConfigureComponent<Position>(x =>
            {
                x.Value = e.Position;
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