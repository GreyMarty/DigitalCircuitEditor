using QuickGraph;

namespace Editor.DecisionDiagrams.Layout.Extensions;

internal static class DiagramExtensions
{
    public static BidirectionalGraph<TVertex, TEdge> ToGraph<TVertex, TEdge>(this INode root, Func<INode, TVertex> vertexFactory, Func<TVertex, TVertex, TEdge> edgeFactory) 
        where TEdge : IEdge<TVertex>
    {
        var graph = new BidirectionalGraph<TVertex, TEdge>();
        root.ToGraph(graph, vertexFactory, edgeFactory, []);
        return graph;
    }
    
    private static TVertex ToGraph<TVertex, TEdge>(this INode root, BidirectionalGraph<TVertex, TEdge> graph, Func<INode, TVertex> vertexFactory, Func<TVertex, TVertex, TEdge> edgeFactory, Dictionary<INode, TVertex> cache) 
        where TEdge : IEdge<TVertex>
    {
        if (cache.TryGetValue(root, out var cached))
        {
            return cached;
        }
        
        var vertex = vertexFactory(root);
        graph.AddVertex(vertex);
        cache[root] = vertex;

        if (root.IsTerminal)
        {
            return vertex;
        }

        var branchNode = (BranchNode)root;
        
        var child1 = branchNode.False.ToGraph(graph, vertexFactory, edgeFactory, cache);
        var child2 = branchNode.True.ToGraph(graph, vertexFactory, edgeFactory, cache);

        graph.AddEdge(edgeFactory(vertex, child1));
        graph.AddEdge(edgeFactory(vertex, child2));

        return vertex;
    }
}