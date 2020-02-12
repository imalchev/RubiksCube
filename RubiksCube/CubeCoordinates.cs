using System;

namespace RubiksCube
{
    public class CubeCoordinates : IEquatable<CubeCoordinates>
    {
        public int X { get; } 
        public int Y { get; }
        public int Z { get; }

        public CubeCoordinates(int x, int y, int z) 
        {
            if(x < 0 || x > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if(y < 0 || y > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            if(z < 0 || z > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            X = x;
            Y = y;
            Z = z;
        }
    
        public static implicit operator CubeCoordinates((int, int, int) tuple) 
            => new CubeCoordinates(tuple.Item1, tuple.Item2, tuple.Item3);

        /// <summary>
        /// Does that coordinates have 1 outer wall
        /// </summary>
        public bool HasOneOuterTale => (X % 2) + (Y % 2) + (Z % 2) == 2;

        /// <summary>
        /// Does that coordinates have 2 outer walls
        /// </summary>
        public bool HasTwoOuterTales => (X % 2) + (Y % 2) + (Z % 2) == 1;

        /// <summary>
        /// Does that coordinates have 3 outer walls
        /// </summary>
        public bool HasThreeOuterTales => 
            X % 2 == 0 
            && Y % 2 == 0 
            && Z % 2 == 0;

        public bool Equals(CubeCoordinates? other)
        {
             if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object? obj) => Equals(obj as CubeCoordinates);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public static bool operator == (CubeCoordinates? coordinates1, CubeCoordinates? coordinates2)
        {
            if (ReferenceEquals(coordinates1, null))
            {
                if (ReferenceEquals(coordinates2, null))
                {
                    return true;
                }

                return false;
            }

            return coordinates1.Equals(coordinates2);
        }

        public static bool operator != (CubeCoordinates? coordinates1, CubeCoordinates? coordinates2)
        {
            return !(coordinates1 == coordinates2);
        }
    }
}
