using System.Collections.Generic;

namespace RubiksCube.Solver
{
    /// <summary>
    /// The outcome of a solve attempt: whether a solution was found, the moves that produce it
    /// (in order, starting from the state passed to <see cref="ICubeSolver.Solve" />), and how many
    /// states the solver looked at while searching.
    /// </summary>
    public sealed class CubeSolverResult
    {
        public bool Solved { get; }
        public IReadOnlyList<RotationCommand> Moves { get; }
        public long StatesExplored { get; }

        public CubeSolverResult(bool solved, IReadOnlyList<RotationCommand> moves, long statesExplored)
        {
            Solved = solved;
            Moves = moves;
            StatesExplored = statesExplored;
        }
    }
}
