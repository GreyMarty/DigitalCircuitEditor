using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Diagrams;

namespace Editor.Core.Behaviors;

public class SelectDiagramBehavior : BehaviorBase<EditorContext>
{
    private BranchNode _branchNodeComponent = default!;
    
    
    protected override void OnInit()
    {
        base.OnInit();
        _branchNodeComponent = Entity.GetRequiredComponent<BranchNode>()!;
    }

    protected override void Perform()
    {
        SelectSubDiagram(_branchNodeComponent);
    }

    private void SelectSubDiagram(Node root)
    {
        root.Entity.GetRequiredComponent<Selectable>().Component!.Selected = true;

        if (root is not BranchNode bn)
        {
            return;
        }
            
        foreach (var (_, connection) in bn.Connections)
        {
            var nextConnection = connection;
            
            while (true)
            {
                nextConnection!.Entity.GetRequiredComponent<Selectable>().Component!.Selected = true;
                var target = nextConnection.Target;

                if (target?.GetComponent<ConnectionJoint>()?.Component is not { } jointComponent)
                {
                    break;
                }
                
                target.GetRequiredComponent<Selectable>().Component!.Selected = true;
                nextConnection = jointComponent.Connection2.GetRequiredComponent<Connection>();
            }
        }
        
        foreach (var (_, node) in bn.Nodes)
        {
            SelectSubDiagram(node);
        }
    }
}