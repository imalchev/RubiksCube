using RubiksCube;
using Xunit;

namespace RubiksCube.Solver.Test
{
    internal static class CubeSolverTestHelpers
    {
        /// <summary>
        /// Runs the solver against <paramref name="scrambled" />, asserts it reports success, and
        /// replays the returned moves to confirm they actually reach a solved cube.
        /// </summary>
        public static CubeSolverResult AssertSolves(ICubeSolver solver, Cube scrambled)
        {
            var result = solver.Solve(scrambled);

            Assert.True(result.Solved);

            var replayed = scrambled;
            foreach (var move in result.Moves)
            {
                replayed = replayed.Rotate(move);
            }

            Assert.True(SolvedCube.IsSolved(replayed));

            return result;
        }
    }
}
