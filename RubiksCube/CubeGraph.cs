using System.Collections.Generic;

namespace RubiksCube
{
    /// <summary>
    /// A single directed edge of the rotation graph: applying <see cref="Move" /> to the state
    /// identified by <see cref="From" /> produces the state identified by <see cref="To" />.
    /// </summary>
    public sealed class CubeGraphEdge
    {
        public CubeStateId From { get; }
        public RotationCommand Move { get; }
        public CubeStateId To { get; }

        public CubeGraphEdge(CubeStateId from, RotationCommand move, CubeStateId to)
        {
            From = from;
            Move = move;
            To = to;
        }
    }

    /// <summary>
    /// A portion of the full rotation graph, explored breadth-first from a starting state up to a
    /// bounded depth. The full graph (every reachable cube state) is far too large to hold in
    /// memory, so <see cref="CubeGraphBuilder" /> only ever materializes a bounded neighborhood of it.
    /// </summary>
    public sealed class CubeGraph
    {
        public IReadOnlyDictionary<CubeStateId, Cube> Vertices { get; }
        public IReadOnlyList<CubeGraphEdge> Edges { get; }

        public CubeGraph(IReadOnlyDictionary<CubeStateId, Cube> vertices, IReadOnlyList<CubeGraphEdge> edges)
        {
            Vertices = vertices;
            Edges = edges;
        }
    }

    public static class CubeGraphBuilder
    {
        /// <summary>
        /// Explores the rotation graph breadth-first starting at <paramref name="start" />, up to
        /// <paramref name="maxDepth" /> moves away. Every state reached within that depth becomes a
        /// vertex; every elementary rotation tried along the way becomes an edge, including edges
        /// that lead back into an already-discovered vertex (those are not expanded further).
        /// </summary>
        public static CubeGraph BuildBreadthFirst(Cube start, int maxDepth)
        {
            var startId = start.GetStateId();

            var vertices = new Dictionary<CubeStateId, Cube> { [startId] = start };
            var edges = new List<CubeGraphEdge>();

            var frontier = new List<(CubeStateId Id, Cube Cube)> { (startId, start) };

            for (int depth = 0; depth < maxDepth && frontier.Count > 0; depth++)
            {
                var nextFrontier = new List<(CubeStateId Id, Cube Cube)>();

                foreach (var (id, cube) in frontier)
                {
                    foreach (var (move, result) in cube.GetNeighbors())
                    {
                        var resultId = result.GetStateId();

                        edges.Add(new CubeGraphEdge(id, move, resultId));

                        if (!vertices.ContainsKey(resultId))
                        {
                            vertices[resultId] = result;
                            nextFrontier.Add((resultId, result));
                        }
                    }
                }

                frontier = nextFrontier;
            }

            return new CubeGraph(vertices, edges);
        }
    }
}
