namespace Editor.DecisionDiagrams;

public class DecisionDiagram(int outputId, INode root)
{
    public int OutputId { get; set; } = outputId;
    public INode Root { get; set; } = root;
}