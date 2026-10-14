using RubiksCube;
using Xunit;

namespace RubiksCube.Solver.Test
{
    public class IdaStarSolverTests
    {
        [Fact]
        public void Solve_AlreadySolvedCube_ReturnsNoMoves()
        {
            var solver = new IdaStarSolver();

            var result = solver.Solve(SolvedCube.Instance);

            Assert.True(result.Solved);
            Assert.Empty(result.Moves);
        }

        [Fact]
        public void Solve_OneMoveScramble_FindsTheSingleInverseMove()
        {
            var solver = new IdaStarSolver();
            var move = new RotationCommand(Orientation.YellowWhite, RotationSegment.First, 1);
            var scrambled = SolvedCube.Instance.Rotate(move);

            var result = CubeSolverTestHelpers.AssertSolves(solver, scrambled);

            Assert.Single(result.Moves);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void Solve_ShortScramble_FindsAWorkingSolution(int scrambleLength)
        {
            var solver = new IdaStarSolver(maxMoves: 6);
            var scrambled = Scrambles.Fixed(scrambleLength);

            var result = CubeSolverTestHelpers.AssertSolves(solver, scrambled);

            Assert.True(result.Moves.Count <= scrambleLength);
        }
    }
}
