using System.Collections.Generic;

namespace RubiksCube.Solver
{
    /// <summary>
    /// Breadth-first search run simultaneously from the start state and from the solved state,
    /// stopping as soon as the two frontiers meet. The meeting point splits the solution into a
    /// forward half (start -> meeting point) and a backward half (meeting point -> solved), the
    /// latter reconstructed by inverting the moves used to reach each backward-frontier state from
    /// the solved cube.
    ///
    /// Optimal: since both searches expand breadth-first and stop at the first intersection, the
    /// returned solution has the fewest possible moves. Only practical for scrambles within
    /// <c>2 * maxDepthPerSide</c> moves of solved, since each extra layer multiplies the frontier
    /// size by up to 18 (the branching factor of <see cref="RotationCommand.AllMoves" />).
    /// </summary>
    public sealed class BidirectionalBfsSolver : ICubeSolver
    {
        private readonly int _maxDepthPerSide;

        public BidirectionalBfsSolver(int maxDepthPerSide = 6)
        {
            _maxDepthPerSide = maxDepthPerSide;
        }

        private sealed class VisitedState
        {
            public Cube Cube { get; }
            public CubeStateId Parent { get; }
            public RotationCommand? MoveFromParent { get; }

            public VisitedState(Cube cube, CubeStateId parent, RotationCommand? moveFromParent)
            {
                Cube = cube;
                Parent = parent;
                MoveFromParent = moveFromParent;
            }
        }

        public CubeSolverResult Solve(Cube start)
        {
            var startId = start.GetStateId();
            var goal = SolvedCube.Instance;
            var goalId = goal.GetStateId();

            if (startId == goalId)
            {
                return new CubeSolverResult(true, new List<RotationCommand>(), 1);
            }

            var forwardVisited = new Dictionary<CubeStateId, VisitedState> { [startId] = new VisitedState(start, startId, null) };
            var backwardVisited = new Dictionary<CubeStateId, VisitedState> { [goalId] = new VisitedState(goal, goalId, null) };

            var forwardFrontier = new List<(CubeStateId Id, Cube Cube)> { (startId, start) };
            var backwardFrontier = new List<(CubeStateId Id, Cube Cube)> { (goalId, goal) };

            long statesExplored = 2;

            for (int depth = 0; depth < _maxDepthPerSide; depth++)
            {
                var meeting = ExpandFrontier(ref forwardFrontier, forwardVisited, backwardVisited, ref statesExplored);
                if (meeting.HasValue)
                {
                    return BuildResult(meeting.Value, forwardVisited, backwardVisited, statesExplored);
                }

                meeting = ExpandFrontier(ref backwardFrontier, backwardVisited, forwardVisited, ref statesExplored);
                if (meeting.HasValue)
                {
                    return BuildResult(meeting.Value, forwardVisited, backwardVisited, statesExplored);
                }
            }

            return new CubeSolverResult(false, new List<RotationCommand>(), statesExplored);
        }

        /// <summary>
        /// Expands every state in <paramref name="frontier" /> by one move, recording newly seen
        /// states in <paramref name="ownVisited" />. Returns the id of the first state found that is
        /// already present in <paramref name="otherVisited" /> (the two searches met), if any.
        /// </summary>
        private static CubeStateId? ExpandFrontier(
            ref List<(CubeStateId Id, Cube Cube)> frontier,
            Dictionary<CubeStateId, VisitedState> ownVisited,
            Dictionary<CubeStateId, VisitedState> otherVisited,
            ref long statesExplored)
        {
            var nextFrontier = new List<(CubeStateId Id, Cube Cube)>();

            foreach (var (id, cube) in frontier)
            {
                foreach (var (move, result) in cube.GetNeighbors())
                {
                    var resultId = result.GetStateId();
                    statesExplored++;

                    if (ownVisited.ContainsKey(resultId))
                    {
                        continue;
                    }

                    ownVisited[resultId] = new VisitedState(result, id, move);
                    nextFrontier.Add((resultId, result));

                    if (otherVisited.ContainsKey(resultId))
                    {
                        frontier = nextFrontier;
                        return resultId;
                    }
                }
            }

            frontier = nextFrontier;
            return null;
        }

        private static CubeSolverResult BuildResult(
            CubeStateId meetingId,
            Dictionary<CubeStateId, VisitedState> forwardVisited,
            Dictionary<CubeStateId, VisitedState> backwardVisited,
            long statesExplored)
        {
            var moves = new List<RotationCommand>();

            // Walk from the meeting point back to the start, forward moves recorded parent -> child,
            // so prepending each one as we go up yields start -> meeting in order.
            var current = meetingId;
            while (forwardVisited[current].MoveFromParent is RotationCommand forwardMove)
            {
                moves.Insert(0, forwardMove);
                current = forwardVisited[current].Parent;
            }

            // Walk from the meeting point back to the goal; each step was recorded as
            // parent --move--> child while searching backward from the goal, so undoing it with the
            // move's inverse goes child -> parent, i.e. one step closer to the goal.
            current = meetingId;
            while (backwardVisited[current].MoveFromParent is RotationCommand backwardMove)
            {
                moves.Add(backwardMove.Inverse());
                current = backwardVisited[current].Parent;
            }

            return new CubeSolverResult(true, moves, statesExplored);
        }
    }
}
