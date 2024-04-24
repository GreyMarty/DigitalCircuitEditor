using Editor.DecisionDiagrams;

namespace Editor.Core.Layout;

public interface ILayout
{
    public NodeLayoutInfo Arrange(BranchNode root);
}