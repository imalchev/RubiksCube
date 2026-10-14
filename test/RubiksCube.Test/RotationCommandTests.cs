using System.Collections.Generic;
using RubiksCube;
using Xunit;

namespace RubiksCube.Test
{
    public class RotationCommandTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Inverse_UndoesTheRotation(int turnovers)
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();
            var command = new RotationCommand(Orientation.RedOrange, RotationSegment.First, turnovers);

            var rotatedCube = orderedCube.Rotate(command);
            var undoneCube = rotatedCube.Rotate(command.Inverse());

            Assert.Equal(orderedCube.GetStateId(), undoneCube.GetStateId());
        }

        [Fact]
        public void AllMoves_Has18ElementaryRotations()
        {
            Assert.Equal(18, RotationCommand.AllMoves.Count);
        }

        [Fact]
        public void AllMoves_AreAllDistinct()
        {
            Assert.Equal(RotationCommand.AllMoves.Count, new HashSet<RotationCommand>(RotationCommand.AllMoves).Count);
        }
    }
}
