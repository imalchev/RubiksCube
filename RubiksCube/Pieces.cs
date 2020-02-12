using System;

namespace RubiksCube
{
    [Flags]
    public enum Color
    {
        White =     1 << 0,
        Green =     1 << 1,
        Orange =    1 << 2,
        Blue =      1 << 3,
        Red =       1 << 4,
        Yellow =    1 << 5
    }

    [Flags]
    public enum Orientation
    {
        X = 2 << 0,
        Y = 2 << 1,
        Z = 2 << 2,
    }

    public enum ThreeTaleOrientation
    {
        /// <summary>
        /// Tale1 is on X axis, Tale2 is on Y axis, Tale3 is on Z axis
        /// </summary>
        XYZ = 1,

        /// <summary>
        /// Tale1 is on X axis, Tale2 is on Z axis, Tale3 is on Y axis
        /// </summary>
        XZY = 2,

        /// <summary>
        /// Tale1 is on Y axis, Tale2 is on X axis, Tale3 is on Z axis
        /// </summary>
        YXZ = 3,

        /// <summary>
        /// Tale1 is on Y axis, Tale2 is on Z axis, Tale3 is on X axis
        /// </summary>
        YZX = 4,

        /// <summary>
        /// Tale1 is on Z axis, Tale2 is on X axis, Tale3 is on Y axis
        /// </summary>
        ZXY = 5,

        /// <summary>
        /// Tale1 is on Z axis, Tale2 is on Y axis, Tale3 is on X axis
        /// </summary>
        ZYX = 6
    }

    public static class Pieces
    {
        public static CentralPiece Yellow { get; } = new CentralPiece(Color.Yellow);
        public static CentralPiece Red { get; } = new CentralPiece(Color.Red);
        public static CentralPiece Blue { get; } = new CentralPiece(Color.Blue);
        public static CentralPiece Green { get; } = new CentralPiece(Color.Green);
        public static CentralPiece White { get; } = new CentralPiece(Color.White);
        public static CentralPiece Orange { get; } = new CentralPiece(Color.Orange);

        public static TwoCornerPiece YellowRed { get; } = new TwoCornerPiece(Color.Yellow, Color.Red);
        public static TwoCornerPiece YellowGreen { get; } = new TwoCornerPiece(Color.Yellow, Color.Green);
        public static TwoCornerPiece YellowBlue { get; } = new TwoCornerPiece(Color.Yellow, Color.Blue);
        public static TwoCornerPiece YellowOrange { get; } = new TwoCornerPiece(Color.Yellow, Color.Orange);
        public static TwoCornerPiece RedGreen { get; } = new TwoCornerPiece(Color.Red, Color.Green);
        public static TwoCornerPiece RedBlue { get; } = new TwoCornerPiece(Color.Red, Color.Blue);
        public static TwoCornerPiece RedWhite { get; } = new TwoCornerPiece(Color.Red, Color.White);
        public static TwoCornerPiece BlueWhite { get; } = new TwoCornerPiece(Color.Blue, Color.White);
        public static TwoCornerPiece BlueOrange { get; } = new TwoCornerPiece(Color.Blue, Color.Orange);
        public static TwoCornerPiece GreenOrange { get; } = new TwoCornerPiece(Color.Green, Color.Orange);
        public static TwoCornerPiece GreenWhite { get; } = new TwoCornerPiece(Color.Green, Color.White);
        public static TwoCornerPiece OrangeWhite { get; } = new TwoCornerPiece(Color.Orange, Color.White);

        public static ThreeCornerPiece YellowRedBlue { get; } = new ThreeCornerPiece(Color.Yellow, Color.Red, Color.Blue);
        public static ThreeCornerPiece YellowRedGreen { get; } = new ThreeCornerPiece(Color.Yellow, Color.Red, Color.Green);
        public static ThreeCornerPiece YellowGreenOrange { get; } = new ThreeCornerPiece(Color.Yellow, Color.Green, Color.Orange);
        public static ThreeCornerPiece YellowBlueOrange { get; } = new ThreeCornerPiece(Color.Yellow, Color.Blue, Color.Orange);
        public static ThreeCornerPiece RedGreenWhite { get; } = new ThreeCornerPiece(Color.Red, Color.Green, Color.White);
        public static ThreeCornerPiece RedBlueWhite { get; } = new ThreeCornerPiece(Color.Red, Color.Blue, Color.White);
        public static ThreeCornerPiece BlueOrangeWhite { get; } = new ThreeCornerPiece(Color.Blue, Color.Orange, Color.White);
        public static ThreeCornerPiece GreenOrangeWhite { get; } = new ThreeCornerPiece(Color.Green, Color.Orange, Color.White);
    }

    public interface IPiece
    {
        Color GetColor(Orientation orientation);
    }

    public sealed class CentralPiece : IPiece, IEquatable<CentralPiece>
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

        public Color GetColor(Orientation orientation)
        {
            throw new NotImplementedException();
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

    public sealed class TwoCornerPiece : IPiece, IEquatable<TwoCornerPiece>
    {        
        public Color Tale1 { get; }
        public Color Tale2 { get; }
    
        public TwoCornerPiece(Color color1, Color color2)
        {
            if (color1 == color2)
            {
                throw new ArgumentException($"The colors can't be equal! {nameof(color1)} is equal to {nameof(color2)}", nameof(color2));
            }

            Tale1 = color1;
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

        public Color GetColor(Orientation orientation)
        {
            throw new NotImplementedException();
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

    public sealed class ThreeCornerPiece : IPiece, IEquatable<ThreeCornerPiece>
    {
        public Color Tale1 { get; }
        public Color Tale2 { get; }
        public Color Tale3 { get; }

        public ThreeCornerPiece(Color color1, Color color2, Color color3)
        {
            if (color1 == color2)
            {
                throw new ArgumentException($"The colors can't be equal! {nameof(color1)} is equal to {nameof(color2)}", nameof(color2));
            }

            if (color1 == color3)
            {
                throw new ArgumentException($"The colors can't be equal! {nameof(color1)} is equal to {nameof(color3)}", nameof(color2));
            }

            if (color2 == color3)
            {
                throw new ArgumentException($"The colors can't be equal! {nameof(color2)} is equal to {nameof(color3)}", nameof(color3));
            }            

            Tale1 = color1;
            Tale2 = color2;
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

        public Color GetColor(Orientation orientation)
        {
            throw new NotImplementedException();
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