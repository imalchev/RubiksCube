using System.Linq;
using RubiksCube;
using Xunit;

namespace RubiksCube.Test
{
    public class CubeGraphTests
    {
        [Fact]
        public void BuildBreadthFirst_AtDepthZero_ContainsOnlyTheStartingState()
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();

            var graph = CubeGraphBuilder.BuildBreadthFirst(orderedCube, maxDepth: 0);

            Assert.Single(graph.Vertices);
            Assert.Equal(orderedCube.GetStateId(), graph.Vertices.Keys.Single());
            Assert.Empty(graph.Edges);
        }

        [Fact]
        public void BuildBreadthFirst_AtDepthOne_HasOneVertexPerElementaryMovePlusTheStart()
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();
            var startId = orderedCube.GetStateId();

            var graph = CubeGraphBuilder.BuildBreadthFirst(orderedCube, maxDepth: 1);

            // every one of the 18 elementary moves applied to the solved cube produces a distinct state
            Assert.Equal(RotationCommand.AllMoves.Count + 1, graph.Vertices.Count);
            Assert.Equal(RotationCommand.AllMoves.Count, graph.Edges.Count);
            Assert.All(graph.Edges, edge => Assert.Equal(startId, edge.From));
        }

        [Fact]
        public void BuildBreadthFirst_EveryEdge_ConnectsStatesPresentInTheGraph()
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();

            var graph = CubeGraphBuilder.BuildBreadthFirst(orderedCube, maxDepth: 2);

            Assert.All(graph.Edges, edge =>
            {
                Assert.True(graph.Vertices.ContainsKey(edge.From));
                Assert.True(graph.Vertices.ContainsKey(edge.To));
            });
        }

        [Fact]
        public void BuildBreadthFirst_ApplyingAnEdgesMove_ReachesItsDeclaredToState()
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();

            var graph = CubeGraphBuilder.BuildBreadthFirst(orderedCube, maxDepth: 2);

            Assert.All(graph.Edges, edge =>
            {
                var fromCube = graph.Vertices[edge.From];
                var actualTo = fromCube.Rotate(edge.Move).GetStateId();

                Assert.Equal(edge.To, actualTo);
            });
        }
    }
}
