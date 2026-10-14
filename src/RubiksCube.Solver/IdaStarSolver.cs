using System;
using System.Collections.Generic;
using System.Linq;

namespace RubiksCube.Solver
{
    /// <summary>
    /// Iterative-deepening A*: depth-first search bounded by a cost threshold (path length so far
    /// plus a heuristic estimate of the remaining distance), with the threshold raised to the
    /// smallest over-the-limit cost seen and the search retried, until a solution is found.
    ///
    /// Memory stays proportional to search depth (unlike plain BFS/A*), so unlike
    /// <see cref="BidirectionalBfsSolver" /> it can be pointed at scrambles of arbitrary depth - at
    /// the cost of revisiting the same shallow states on every iteration, which the heuristic exists
    /// to limit.
    /// </summary>
    public sealed class IdaStarSolver : ICubeSolver
    {
        private readonly int _maxMoves;
        private long _statesExplored;

        public IdaStarSolver(int maxMoves = 20)
        {
            _maxMoves = maxMoves;
        }

        public CubeSolverResult Solve(Cube start)
        {
            _statesExplored = 0;

            var threshold = Heuristic(start);
            var path = new List<RotationCommand>();

            while (threshold <= _maxMoves)
            {
                var result = Search(start, 0, threshold, path, null);

                if (result == FoundMarker)
                {
                    return new CubeSolverResult(true, path, _statesExplored);
                }

                if (result == int.MaxValue)
                {
                    break;
                }

                threshold = result;
            }

            return new CubeSolverResult(false, new List<RotationCommand>(), _statesExplored);
        }

        private const int FoundMarker = -1;

        /// <summary>
        /// Depth-first search of the current branch. Returns <see cref="FoundMarker"/> once a
        /// solution is appended to <paramref name="path" />; otherwise returns the smallest cost
        /// (moves so far + heuristic) seen beyond <paramref name="threshold" />, to be used as next
        /// iteration's threshold, or <see cref="int.MaxValue" /> if the branch is a dead end within
        /// <see cref="_maxMoves" />.
        /// </summary>
        private int Search(Cube cube, int movesSoFar, int threshold, List<RotationCommand> path, RotationCommand? lastMove)
        {
            _statesExplored++;

            var estimatedTotal = movesSoFar + Heuristic(cube);
            if (estimatedTotal > threshold)
            {
                return estimatedTotal;
            }

            if (SolvedCube.IsSolved(cube))
            {
                return FoundMarker;
            }

            if (movesSoFar >= _maxMoves)
            {
                return int.MaxValue;
            }

            var min = int.MaxValue;

            foreach (var move in RotationCommand.AllMoves)
            {
                // Undoing the move just made can never be part of a shortest path.
                if (lastMove != null && move.Equals(lastMove.Inverse()))
                {
                    continue;
                }

                path.Add(move);
                var result = Search(cube.Rotate(move), movesSoFar + 1, threshold, path, move);

                if (result == FoundMarker)
                {
                    return FoundMarker;
                }

                path.RemoveAt(path.Count - 1);

                if (result < min)
                {
                    min = result;
                }
            }

            return min;
        }

        /// <summary>
        /// Admissible lower bound on the remaining moves: the number of movable pieces not yet in
        /// their solved slot/orientation, divided by 8 (the most pieces a single quarter- or
        /// half-turn can move into place).
        /// </summary>
        private static int Heuristic(Cube cube)
        {
            var misplaced = 0;

            foreach (var slot in MovableSlots)
            {
                var piece = cube.GetCubePiece(slot);
                var solvedPiece = SolvedCube.Instance.GetCubePiece(slot);

                if (!Matches(piece, solvedPiece, slot))
                {
                    misplaced++;
                }
            }

            return (int)Math.Ceiling(misplaced / 8.0);
        }

        private static bool Matches(CubePiece piece, CubePiece solvedPiece, CubeCoordinates slot)
        {
            if (!piece.Piece.Equals(solvedPiece.Piece))
            {
                return false;
            }

            foreach (var axis in OuterOrientations(slot))
            {
                if (piece.GetColor(axis) != solvedPiece.GetColor(axis))
                {
                    return false;
                }
            }

            return true;
        }

        private static IEnumerable<Orientation> OuterOrientations(CubeCoordinates slot)
        {
            if (slot.X == 0 || slot.X == 2)
            {
                yield return Orientation.GreenBlue;
            }

            if (slot.Y == 0 || slot.Y == 2)
            {
                yield return Orientation.YellowWhite;
            }

            if (slot.Z == 0 || slot.Z == 2)
            {
                yield return Orientation.RedOrange;
            }
        }

        private static readonly IReadOnlyList<CubeCoordinates> MovableSlots = BuildMovableSlots();

        private static List<CubeCoordinates> BuildMovableSlots()
        {
            var slots = new List<CubeCoordinates>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        CubeCoordinates coordinate = (x, y, z);

                        if (coordinate.HasTwoOuterTales || coordinate.HasThreeOuterTales)
                        {
                            slots.Add(coordinate);
                        }
                    }
                }
            }

            return slots;
        }
    }
}
