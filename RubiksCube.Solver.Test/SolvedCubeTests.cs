using RubiksCube;
using RubiksCube.Solver;
using Xunit;

namespace RubiksCube.Solver.Test
{
    public class SolvedCubeTests
    {
        [Fact]
        public void IsSolved_OrderedCube_IsTrue()
        {
            Assert.True(SolvedCube.IsSolved(SolvedCube.Instance));
        }

        [Theory]
        [InlineData(Orientation.YellowWhite)]
        [InlineData(Orientation.RedOrange)]
        [InlineData(Orientation.GreenBlue)]
        public void IsSolved_AfterASingleQuarterTurn_IsFalse(Orientation axis)
        {
            var scrambled = SolvedCube.Instance.Rotate(new RotationCommand(axis, RotationSegment.First, 1));

            Assert.False(SolvedCube.IsSolved(scrambled));
        }

        [Theory]
        [InlineData(Orientation.YellowWhite)]
        [InlineData(Orientation.RedOrange)]
        [InlineData(Orientation.GreenBlue)]
        public void IsSolved_AfterFourQuarterTurns_IsTrueAgain(Orientation axis)
        {
            var cube = SolvedCube.Instance;

            for (int i = 0; i < 4; i++)
            {
                cube = cube.Rotate(new RotationCommand(axis, RotationSegment.First, 1));
            }

            Assert.True(SolvedCube.IsSolved(cube));
        }
    }
}
