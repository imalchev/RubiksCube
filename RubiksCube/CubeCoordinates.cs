using System;

namespace RubiksCube
{
    public class CubeCoordinates : IEquatable<CubeCoordinates>
    {
        public static CubeCoordinates Center = new CubeCoordinates(1, 1, 1);

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

        public bool IsTwoColors => (X % 2) + (Y % 2) + (Z % 2) == 1;

        public bool IsThreeColors => 
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
                if(ReferenceEquals(coordinates2, null))
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
