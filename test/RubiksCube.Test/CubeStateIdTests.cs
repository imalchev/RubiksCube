using RubiksCube;
using Xunit;

namespace RubiksCube.Test
{
    public class CubeStateIdTests
    {
        [Fact]
        public void SameState_ProducesEqualIds()
        {
            var cube1 = CubeBuilder.CreateOrdered().Build();
            var cube2 = CubeBuilder.CreateOrdered().Build();

            Assert.Equal(cube1.GetStateId(), cube2.GetStateId());
        }

        [Fact]
        public void RotatedState_ProducesDifferentId()
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();
            var rotatedCube = orderedCube.Rotate(new RotationCommand(Orientation.YellowWhite, RotationSegment.First, turnovers: 1));

            Assert.NotEqual(orderedCube.GetStateId(), rotatedCube.GetStateId());
        }

        [Fact]
        public void RotationFollowedByItsInverse_ReturnsToOriginalId()
        {
            var orderedCube = CubeBuilder.CreateOrdered().Build();

            var rotatedCube = orderedCube.Rotate(new RotationCommand(Orientation.YellowWhite, RotationSegment.First, turnovers: 1));
            var undoneCube = rotatedCube.Rotate(new RotationCommand(Orientation.YellowWhite, RotationSegment.First, turnovers: 3));

            Assert.Equal(orderedCube.GetStateId(), undoneCube.GetStateId());
        }
    }
}
