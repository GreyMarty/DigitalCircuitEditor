using System.Numerics;
using Editor.DecisionDiagrams.Circuits;
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
    
    
    public LayoutInfo Arrange(INode root)
    {
        var graph = root.ToGraph(
            x => new LayoutVertex(x.Id),
            (a, b) => new LayoutEdge(a, b)
        );

        
        var vertexSizes = graph.Vertices.ToDictionary(x => x, _ => new Size(3, 3));
        
        return Arrange(graph, vertexSizes);
    }

    public LayoutInfo Arrange(ICircuitElement root)
    {
        var graph = root.ToGraph(
            x => new LayoutVertex(x.Id),
            (a, b) => new LayoutEdge(a, b)
        );
        
        var vertexSizes = graph.Vertices.ToDictionary(x => x, x => new Size(4, 4));
        
        return Arrange(graph, vertexSizes);
    }

    private LayoutInfo Arrange(BidirectionalGraph<LayoutVertex, LayoutEdge> graph, Dictionary<LayoutVertex, Size> vertexSizes)
    {
        if (graph.VertexCount == 1)
        {
            return new LayoutInfo(
                new Dictionary<int, Vector2>
                {
                    [graph.Vertices.First().NodeId] = Vector2.Zero
                },
                []
            );
        }
        
        var layout = new EfficientSugiyamaLayoutAlgorithm<LayoutVertex, LayoutEdge, BidirectionalGraph<LayoutVertex, LayoutEdge>>(graph, Parameters, vertexSizes);
        
        layout.Compute(CancellationToken.None);
        
        return new LayoutInfo(
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