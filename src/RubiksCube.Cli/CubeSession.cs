using System;
using System.Collections.Generic;
using RubiksCube;

namespace RubiksCube.Cli
{
    /// <summary>
    /// The CLI's mutable session state: the current cube (if any) and the moves applied to it since
    /// the last <see cref="Init" />. <see cref="Cube" /> itself is immutable, so applying a move
    /// just reassigns <see cref="Current" /> to the new state.
    /// </summary>
    public sealed class CubeSession
    {
        private readonly List<RotationCommand> _history = new List<RotationCommand>();

        public Cube? Current { get; private set; }

        public IReadOnlyList<RotationCommand> History => _history;

        public void Init(Cube cube)
        {
            Current = cube;
            _history.Clear();
        }

        public void Apply(RotationCommand move)
        {
            if (Current == null)
            {
                throw new InvalidOperationException("No cube initialized yet. Run 'init' first.");
            }

            Current = Current.Rotate(move);
            _history.Add(move);
        }
    }
}
