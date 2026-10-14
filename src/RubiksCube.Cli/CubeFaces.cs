using System;
using System.Collections.Generic;
using System.Linq;
using RubiksCube;

namespace RubiksCube.Cli
{
    /// <summary>
    /// The single source of truth for how a face's center color maps to the (axis, segment) pair
    /// that identifies it as a <see cref="RotationCommand" /> target — the same pairing
    /// <see cref="Cube.GetSideColors(Color)" /> uses internally to find a face's center coordinate.
    /// Shared by <see cref="MoveNotation" /> (parsing/formatting moves) and
    /// <see cref="CubeFaceRenderer" /> (reading tiles off a face).
    /// </summary>
    internal static class CubeFaces
    {
        public static readonly IReadOnlyList<Color> All = new[]
        {
            Color.Yellow, Color.White, Color.Red, Color.Orange, Color.Green, Color.Blue,
        };

        private static readonly IReadOnlyDictionary<Color, (Orientation Axis, RotationSegment Segment)> Axes =
            new Dictionary<Color, (Orientation, RotationSegment)>
            {
                [Color.Red] = (Orientation.RedOrange, RotationSegment.First),
                [Color.Orange] = (Orientation.RedOrange, RotationSegment.Third),
                [Color.Green] = (Orientation.GreenBlue, RotationSegment.First),
                [Color.Blue] = (Orientation.GreenBlue, RotationSegment.Third),
                [Color.Yellow] = (Orientation.YellowWhite, RotationSegment.First),
                [Color.White] = (Orientation.YellowWhite, RotationSegment.Third),
            };

        private static readonly IReadOnlyDictionary<(Orientation, RotationSegment), Color> Colors =
            Axes.ToDictionary(pair => pair.Value, pair => pair.Key);

        public static (Orientation Axis, RotationSegment Segment) GetAxis(Color faceCenter)
        {
            if (!Axes.TryGetValue(faceCenter, out var axis))
            {
                throw new ArgumentOutOfRangeException(nameof(faceCenter), faceCenter, "Not a face center color.");
            }

            return axis;
        }

        public static Color GetFaceColor(Orientation axis, RotationSegment segment) => Colors[(axis, segment)];

        public static bool IsFaceColor(Color color) => Axes.ContainsKey(color);
    }
}
