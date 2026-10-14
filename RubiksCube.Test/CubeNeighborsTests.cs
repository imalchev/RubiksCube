using System.Linq;
using RubiksCube;
using Xunit;

namespace RubiksCube.Test
{
    public class CubeNeighborsTests
    {
        [Fact]
        public void GetNeighbors_ReturnsOneResultPerElementaryMove()
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();

            var neighbors = orderedCube.GetNeighbors().ToList();

            Assert.Equal(RotationCommand.AllMoves.Count, neighbors.Count);
        }

        [Fact]
        public void GetNeighbors_EachResultUndoesBackToTheOriginalState()
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();
            var originalId = orderedCube.GetStateId();

            foreach (var (move, result) in orderedCube.GetNeighbors())
            {
                var undone = result.Rotate(move.Inverse());

                Assert.Equal(originalId, undone.GetStateId());
            }
        }
    }
}
