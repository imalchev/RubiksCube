using System;
using System.Collections.Generic;

namespace RubiksCube
{
    public enum RotationSegment
    {
        First = 0,
        Third = 2
    }

    /// <summary>
    /// A single elementary rotation of the cube: a graph edge, connecting the state it is applied to
    /// with the state it produces.
    /// </summary>
    public sealed class RotationCommand : IEquatable<RotationCommand>
    {
        /// <summary>
        /// All 18 elementary rotations of the cube: every combination of axis, segment (face) and
        /// turnovers (quarter, half and three-quarter turn).
        /// </summary>
        public static IReadOnlyList<RotationCommand> AllMoves { get; } = BuildAllMoves();

        public Orientation RotationAxis { get; }
        public RotationSegment Segment { get; }
        public int Turnovers { get; }

        public RotationCommand(Orientation rotationAxis, RotationSegment segment, int turnovers = 1)
        {
            RotationAxis = rotationAxis;
            Segment = segment;
            Turnovers = turnovers % 4;
        }

        /// <summary>
        /// Gets the rotation that undoes this one, i.e. applying this command and then its inverse
        /// (or vice versa) returns the cube to its original state.
        /// </summary>
        public RotationCommand Inverse() => new RotationCommand(RotationAxis, Segment, 4 - Turnovers);

        public bool Equals(RotationCommand? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return RotationAxis == other.RotationAxis && Segment == other.Segment && Turnovers == other.Turnovers;
        }

        public override bool Equals(object? obj) => Equals(obj as RotationCommand);

        public override int GetHashCode() => HashCode.Combine(RotationAxis, Segment, Turnovers);

        public override string ToString() => $"{RotationAxis} {Segment} x{Turnovers}";

        public static bool operator ==(RotationCommand? left, RotationCommand? right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(RotationCommand? left, RotationCommand? right) => !(left == right);

        private static IReadOnlyList<RotationCommand> BuildAllMoves()
        {
            var axes = new[] { Orientation.RedOrange, Orientation.YellowWhite, Orientation.GreenBlue };
            var segments = new[] { RotationSegment.First, RotationSegment.Third };
            var turnovers = new[] { 1, 2, 3 };

            var moves = new List<RotationCommand>(axes.Length * segments.Length * turnovers.Length);

            foreach (var axis in axes)
            {
                foreach (var segment in segments)
                {
                    foreach (var turnover in turnovers)
                    {
                        moves.Add(new RotationCommand(axis, segment, turnover));
                    }
                }
            }

            return moves;
        }
    }
}
