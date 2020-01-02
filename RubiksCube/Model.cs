using System;

namespace RubiksCube
{
    [Flags]
    public enum Color
    {
        White = 1 << 0,
        Green = 1 << 1,
        Orange = 1 << 2,
        Blue = 1 << 3,
        Red = 1 << 4,
        Yellow = 1 << 5
    }

    public interface IPice
    {
    }

    public class CentralPiece : IPice, IEquatable<CentralPiece>
    {
        public Color Tale1 { get; }

        public CentralPiece(Color color)
        {
            Tale1 = color;
        }

        public override int GetHashCode()
        {
            return (int)Tale1;
        }

        public override bool Equals(object? obj) => Equals(obj as CentralPiece);

        public bool Equals(CentralPiece? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Tale1 == other.Tale1;
        }

        public static bool operator == (CentralPiece? piece1, CentralPiece? piece2)
        {
            if(ReferenceEquals(piece1, null))
            {
                if(ReferenceEquals(piece2, null))
                {
                    return true;
                }

                return false;
            }

            return piece1.Equals(piece2);
        }

        public static bool operator != (CentralPiece? piece1, CentralPiece? piece2)
        {
            return !(piece1 == piece2);
        }
    }

    public class TwoCornerPiece : CentralPiece, IEquatable<TwoCornerPiece>
    {        
        public Color Tale2 { get; }
    
        public TwoCornerPiece(Color color1, Color color2) 
            : base(color1)
        {
            if (color1 == color2)
            {
                throw new ArgumentException($"The colors can't be equal! {nameof(color1)} is equal to {nameof(color2)}", nameof(color2));
            }

            Tale2 = color2;
        }

        public override int GetHashCode()
        {
            return (int)Tale1 | (int)Tale2;
        }

        public override bool Equals(object? obj) => Equals(obj as TwoCornerPiece);

        public bool Equals(TwoCornerPiece? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ((int)Tale1 | (int)Tale2) == ((int)other.Tale1 | (int)other.Tale2);
        }

        public static bool operator == (TwoCornerPiece? piece1, TwoCornerPiece? piece2)
        {
            if(ReferenceEquals(piece1, null))
            {
                if(ReferenceEquals(piece2, null))
                {
                    return true;
                }

                return false;
            }

            return piece1.Equals(piece2);
        }

        public static bool operator != (TwoCornerPiece? piece1, TwoCornerPiece? piece2)
        {
            return !(piece1 == piece2);
        }
    }

    public class ThreeCornerPiece : TwoCornerPiece, IEquatable<ThreeCornerPiece>
    {
        public Color Tale3 { get; }

        public ThreeCornerPiece(Color color1, Color color2, Color color3)
            : base(color1, color2)
        {
            if (color1 == color3)
            {
                throw new ArgumentException($"The colors can't be equal! {nameof(color1)} is equal to {nameof(color3)}", nameof(color2));
            }

            if (color2 == color3)
            {
                throw new ArgumentException($"The colors can't be equal! {nameof(color2)} is equal to {nameof(color3)}", nameof(color3));
            }

            Tale3 = color3;
        }

        public override int GetHashCode()
        {
            return (int)Tale1 | (int)Tale2 | (int)Tale3;
        }

        public override bool Equals(object? obj) => Equals(obj as ThreeCornerPiece);

        public bool Equals(ThreeCornerPiece? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ((int)Tale1 | (int)Tale2 | (int)Tale3) == ((int)other.Tale1 | (int)other.Tale2 | (int)other.Tale3);
        }

        public static bool operator == (ThreeCornerPiece? piece1, ThreeCornerPiece? piece2)
        {
            if(ReferenceEquals(piece1, null))
            {
                if(ReferenceEquals(piece2, null))
                {
                    return true;
                }

                return false;
            }

            return piece1.Equals(piece2);
        }

        public static bool operator != (ThreeCornerPiece? piece1, ThreeCornerPiece? piece2)
        {
            return !(piece1 == piece2);
        }
    }
}