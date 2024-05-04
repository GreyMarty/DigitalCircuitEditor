using System.Numerics;
using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Adapters;
using Editor.Core.Prefabs.Factories;

namespace Editor.Core.Components.Diagrams;

public class Connection : EditorComponentBase
{
    public IEntity? Target { get; set; }
    public ConnectionType Type { get; set; }

    public string? CustomLabel { get; set; }
    public virtual string? Label
    {
        get => CustomLabel ?? (Type != ConnectionType.Direct ? Type.ToString() : null);
        set { }
    }

    public IEntityBuilderFactory ConnectionJointFactory { get; set; } = new ConnectionJointFactory();
    public IEntityBuilderFactory ConnectionFactory { get; set; } = new ConnectionFactory();
    

    public ConnectionJoint Split(Vector2 position)
    {
        var joint = Context.Instantiate(ConnectionJointFactory.Create()
            .ConfigureComponent<Position>(x =>
            {
                x.Value = position;
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
                x.Target = Target;
                x.Type = Type;
                x.CustomLabel = string.Empty;
            })
        );
        
        Target = joint;

        jointComponent.Connection1 = Entity;
        jointComponent.Connection2 = connection;

        if (jointComponent.Connection1.GetComponent<ConnectionToLineShapeAdapter>()?.Component is { } connection1Adapter)
        {
            connection1Adapter.IgnoreTargetShape = true;
        }
        
        if (jointComponent.Connection2.GetComponent<ConnectionToLineShapeAdapter>()?.Component is { } connection2Adapter)
        {
            connection2Adapter.IgnoreSourceShape = true;
        }
        
        return jointComponent;
    }

    protected override void OnInit()
    {
        Events.Subscribe<EntityDestroyed>(Context_OnEntityDestroyed);
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<EntityDestroyed>();
    }

    private void Context_OnEntityDestroyed(EntityDestroyed e)
    {
        if (e.Entity == Target)
        {
            Context.Destroy(Entity);
        }
    }
}
