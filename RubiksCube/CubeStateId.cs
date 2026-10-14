using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RubiksCube
{
    /// <summary>
    /// A unique, deterministic identifier for a reachable cube state (vertex key for the rotation graph).
    /// Two cubes with the same physical arrangement of pieces produce equal ids, regardless of the
    /// sequence of rotations that led to them.
    /// </summary>
    public readonly struct CubeStateId : IEquatable<CubeStateId>
    {
        // Orientation is a bitflag enum (None/RedOrange/YellowWhite/GreenBlue), so it's remapped
        // to a dense 0-3 index before being used as a digit.
        private static readonly Dictionary<Orientation, int> OrientationIndex = new Dictionary<Orientation, int>
        {
            [Orientation.None] = 0,
            [Orientation.RedOrange] = 1,
            [Orientation.YellowWhite] = 2,
            [Orientation.GreenBlue] = 3,
        };

        private const int OrientationCombinations = 4 * 4 * 4; // one of 4 orientations per tale, 3 tales

        private static readonly IReadOnlyList<CubeCoordinates> MovableSlots = BuildMovableSlots();
        private static readonly IReadOnlyDictionary<Piece, int> PieceIndex = BuildPieceIndex();

        private static readonly BigInteger DigitBase = Pieces.All.Length * OrientationCombinations;

        public BigInteger Value { get; }

        private CubeStateId(BigInteger value)
        {
            Value = value;
        }

        /// <summary>
        /// Computes the state id of <paramref name="cube" /> from the pieces and orientations
        /// currently occupying its 20 movable (corner and edge) slots.
        /// </summary>
        public static CubeStateId Of(Cube cube)
        {
            var id = BigInteger.Zero;

            foreach (var slot in MovableSlots)
            {
                var cubePiece = cube.GetCubePiece(slot);

                var pieceIndex = PieceIndex[cubePiece.Piece];
                var orientationValue =
                    OrientationIndex[cubePiece.Tale1Orientation] * 16
                    + OrientationIndex[cubePiece.Tale2Orientation] * 4
                    + OrientationIndex[cubePiece.Tale3Orientation];

                var digit = pieceIndex * OrientationCombinations + orientationValue;

                id = id * DigitBase + digit;
            }

            return new CubeStateId(id);
        }

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

        private static Dictionary<Piece, int> BuildPieceIndex()
        {
            return Pieces.All
                .Select((piece, index) => (piece, index))
                .ToDictionary(x => x.piece, x => x.index);
        }

        public bool Equals(CubeStateId other) => Value == other.Value;

        public override bool Equals(object? obj) => obj is CubeStateId other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();

        public static bool operator ==(CubeStateId left, CubeStateId right) => left.Equals(right);

        public static bool operator !=(CubeStateId left, CubeStateId right) => !left.Equals(right);
    }
}
