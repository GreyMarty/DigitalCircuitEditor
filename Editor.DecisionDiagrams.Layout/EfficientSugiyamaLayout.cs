using System.Numerics;
using Editor.DecisionDiagrams.Layout.Extensions;
using GraphX.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Measure;
using QuickGraph;

namespace Editor.DecisionDiagrams.Layout;

public class EfficientSugiyamaLayout : ILayout
{
    public EfficientSugiyamaLayoutParameters Parameters { get; set; } = new()
    {
        Direction = LayoutDirection.TopToBottom,
        EdgeRouting = SugiyamaEdgeRoutings.Traditional,
        LayerDistance = 6,
        VertexDistance = 6,
        OptimizeWidth = false
    };
    
    
    public NodeLayoutInfo Arrange(INode root)
    {
        var graph = root.ToGraph(
            x => new LayoutVertex(x.Id),
            (a, b) => new LayoutEdge(a, b)
        );

        var vertexSizes = graph.Vertices.ToDictionary(x => x, _ => new Size(4, 4));
        
        var layout = new EfficientSugiyamaLayoutAlgorithm<LayoutVertex, LayoutEdge, BidirectionalGraph<LayoutVertex, LayoutEdge>>(graph, Parameters, vertexSizes);
        
        layout.Compute(CancellationToken.None);
        
        return new NodeLayoutInfo(
            layout.VertexPositions.ToDictionary(
                x => x.Key.NodeId,
                x => new Vector2((float)x.Value.X, (float)x.Value.Y)
            ),
            layout.EdgeRoutes.ToDictionary(
                x => (x.Key.Source.NodeId, x.Key.Target.NodeId),
                x => x.Value.Select(p => new Vector2((float)p.X, (float)p.Y))
            )
        );
    }
}