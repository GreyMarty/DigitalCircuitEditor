using GraphX.Common.Models;

namespace Editor.DecisionDiagrams.Layout;

public class LayoutEdge(LayoutVertex source, LayoutVertex target, double weight = 1)
    : EdgeBase<LayoutVertex>(source, target, weight);