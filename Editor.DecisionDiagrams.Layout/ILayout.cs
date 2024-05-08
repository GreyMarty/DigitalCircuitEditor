using Editor.DecisionDiagrams.Circuits;

namespace Editor.DecisionDiagrams.Layout;

public interface ILayout
{
    public LayoutInfo Arrange(INode root);
    public LayoutInfo Arrange(ICircuitElement root);
}