using System.Collections.Generic;
using RubiksCube;

namespace RubiksCube.Solver.Test
{
    /// <summary>
    /// Deterministic, non-cancelling scrambles for tests: cycles through axes/segments so that no
    /// move undoes the previous one, keeping the true solve distance equal to the scramble length.
    /// </summary>
    internal static class Scrambles
    {
        private static readonly IReadOnlyList<RotationCommand> Sequence = new List<RotationCommand>
        {
            new RotationCommand(Orientation.YellowWhite, RotationSegment.First, 1),
            new RotationCommand(Orientation.RedOrange, RotationSegment.Third, 1),
            new RotationCommand(Orientation.GreenBlue, RotationSegment.First, 1),
            new RotationCommand(Orientation.YellowWhite, RotationSegment.Third, 2),
            new RotationCommand(Orientation.RedOrange, RotationSegment.First, 1),
        };

        public static Cube Fixed(int length)
        {
            var cube = SolvedCube.Instance;

            for (int i = 0; i < length; i++)
            {
                cube = cube.Rotate(Sequence[i % Sequence.Count]);
            }

            return cube;
        }
    }
}
