using Editor.DecisionDiagrams.Circuits;
using Editor.DecisionDiagrams.Circuits.Gates;
using QuickGraph;

namespace Editor.DecisionDiagrams.Layout.Extensions;

internal static class CircuitExtensions
{
    public static BidirectionalGraph<TVertex, TEdge> ToGraph<TVertex, TEdge>(this ICircuitElement root, Func<ICircuitElement, TVertex> vertexFactory, Func<TVertex, TVertex, TEdge> edgeFactory) 
        where TEdge : IEdge<TVertex>
    {
        var graph = new BidirectionalGraph<TVertex, TEdge>();
        root.ToGraph(graph, vertexFactory, edgeFactory, []);
        return graph;
    }
    
    private static TVertex ToGraph<TVertex, TEdge>(this ICircuitElement root, BidirectionalGraph<TVertex, TEdge> graph, Func<ICircuitElement, TVertex> vertexFactory, Func<TVertex, TVertex, TEdge> edgeFactory, Dictionary<ICircuitElement, TVertex> cache) 
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

        var gate = (ILogicGate)root;

        foreach (var child in gate.Inputs.Reverse())
        {
            var childVertex = child.ToGraph(graph, vertexFactory, edgeFactory, cache);
            graph.AddEdge(edgeFactory(vertex, childVertex));
        }

        return vertex;
    }
}