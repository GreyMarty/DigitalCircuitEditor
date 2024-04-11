namespace Editor.Core.Components.Diagrams;

public class OutputNode : BranchNode
{
    public int OutputId { get; private set; }

    public override string? Label
    {
        get => $"F{OutputId}";
        set { }
    }


    protected override void OnInit()
    {
        base.OnInit();

        var ids = Context.Entities
            .Where(x => x != Entity)
            .Select(x => x.GetComponent<OutputNode>()?.Component)
            .Where(x => x is not null)
            .Select(x => x.OutputId)
            .ToList();
        
        if (ids.Count > 0)
        {
            OutputId = ids.Max() + 1;
        }
    }
}