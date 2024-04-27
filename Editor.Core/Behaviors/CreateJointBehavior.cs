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
        _connectionComponent.Split(e.Position);
    }
}