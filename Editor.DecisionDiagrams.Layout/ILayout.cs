namespace Editor.DecisionDiagrams.Layout;

public interface ILayout
{
    public NodeLayoutInfo Arrange(INode root);
}