using System;

namespace RubiksCube
{
    [Flags]
    public enum Color
    {
        None = 0,
        White =     1 << 0,
        Green =     1 << 1,
        Orange =    1 << 2,
        Blue =      1 << 3,
        Red =       1 << 4,
        Yellow =    1 << 5
    }

    /// <summary>
    /// All the valid orientations of the cube. They match the asses
    /// </summary>
    [Flags]
    public enum Orientation
    {
        None = 0,

        /// <summary>
        /// Represents the asses from red side to orange side
        /// </summary>
        RedOrange = 1 << 0,

        /// <summary>
        /// Represents the asses from yellow side to white side
        /// </summary>
        YellowWhite = 1 << 1,
        
        /// <summary>
        /// Represents the asses from green side to blue side
        /// </summary>
        GreenBlue = 1 << 2,
    }

    /// <summary>
    /// Enumeration of all valid cube pieces
    /// </summary>
    public static class Pieces
    {
        public static Piece Yellow { get; } = Piece.CentralPiece(Color.Yellow);
        public static Piece Red { get; } = Piece.CentralPiece(Color.Red);
        public static Piece Blue { get; } = Piece.CentralPiece(Color.Blue);
        public static Piece Green { get; } = Piece.CentralPiece(Color.Green);
        public static Piece White { get; } = Piece.CentralPiece(Color.White);
        public static Piece Orange { get; } = Piece.CentralPiece(Color.Orange);

        public static Piece YellowRed { get; } = Piece.TwoCornerPiece(Color.Yellow, Color.Red);
        public static Piece YellowGreen { get; } = Piece.TwoCornerPiece(Color.Yellow, Color.Green);
        public static Piece YellowBlue { get; } = Piece.TwoCornerPiece(Color.Yellow, Color.Blue);
        public static Piece YellowOrange { get; } = Piece.TwoCornerPiece(Color.Yellow, Color.Orange);
        public static Piece RedGreen { get; } = Piece.TwoCornerPiece(Color.Red, Color.Green);
        public static Piece RedBlue { get; } = Piece.TwoCornerPiece(Color.Red, Color.Blue);
        public static Piece RedWhite { get; } = Piece.TwoCornerPiece(Color.Red, Color.White);
        public static Piece BlueWhite { get; } = Piece.TwoCornerPiece(Color.Blue, Color.White);
        public static Piece BlueOrange { get; } = Piece.TwoCornerPiece(Color.Blue, Color.Orange);
        public static Piece GreenOrange { get; } = Piece.TwoCornerPiece(Color.Green, Color.Orange);
        public static Piece GreenWhite { get; } = Piece.TwoCornerPiece(Color.Green, Color.White);
        public static Piece OrangeWhite { get; } = Piece.TwoCornerPiece(Color.Orange, Color.White);

        public static Piece YellowRedBlue { get; } = Piece.ThreeCornerPiece(Color.Yellow, Color.Red, Color.Blue);
        public static Piece YellowRedGreen { get; } = Piece.ThreeCornerPiece(Color.Yellow, Color.Red, Color.Green);
        public static Piece YellowGreenOrange { get; } = Piece.ThreeCornerPiece(Color.Yellow, Color.Green, Color.Orange);
        public static Piece YellowBlueOrange { get; } = Piece.ThreeCornerPiece(Color.Yellow, Color.Blue, Color.Orange);
        public static Piece RedGreenWhite { get; } = Piece.ThreeCornerPiece(Color.Red, Color.Green, Color.White);
        public static Piece RedBlueWhite { get; } = Piece.ThreeCornerPiece(Color.Red, Color.Blue, Color.White);
        public static Piece BlueOrangeWhite { get; } = Piece.ThreeCornerPiece(Color.Blue, Color.Orange, Color.White);
        public static Piece GreenOrangeWhite { get; } = Piece.ThreeCornerPiece(Color.Green, Color.Orange, Color.White);
    }


    public sealed class Piece : IEquatable<Piece>
    {
        public Color Tale1 { get; }
        public Color Tale2 { get; }
        public Color Tale3 { get; }

        public static Piece CentralPiece(Color color1)
        {
            if (color1 == Color.None)
            {
                throw new ArgumentException(nameof(color1));
            }

            return new Piece(color1);
        }

        public static Piece TwoCornerPiece(Color color1, Color color2)
        {
            if (color1 == Color.None)
            {
                throw new ArgumentException(nameof(color1));
            }

            if (color2 == Color.None)
            {
                throw new ArgumentException(nameof(color2));
            }

            return new Piece(color1, color2);
        }

        public static Piece ThreeCornerPiece(Color color1, Color color2, Color color3)
        {
            if (color1 == Color.None)
            {
                throw new ArgumentException(nameof(color1));
            }

            if (color2 == Color.None)
            {
                throw new ArgumentException(nameof(color2));
            }

            if (color3 == Color.None)
            {
                throw new ArgumentException(nameof(color3));
            }

            return new Piece(color1, color2, color3);
        }        

        private Piece(Color color1)
        {
            Tale1 = color1;
            Tale2 = Color.None;
            Tale3 = Color.None;
        }

        private Piece(Color color1, Color color2)
        {
            if (color1 == color2)
            {
                throw new ArgumentException($"The colors can't be equal! {nameof(color1)} is equal to {nameof(color2)}", nameof(color2));
            }

            Tale1 = color1;
            Tale2 = color2;
            Tale3 = Color.None;
        }
        
        private Piece(Color color1, Color color2, Color color3)
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

        public bool HasOneOuterTale => 
            Tale1 != Color.None
            && Tale2 == Color.None 
            && Tale3 == Color.None;

        public bool HasTwoOuterTales => 
            Tale1 != Color.None
            && Tale2 != Color.None 
            && Tale3 == Color.None;

        public bool HasThreeOuterTales => 
            Tale1 != Color.None
            && Tale2 != Color.None 
            && Tale3 != Color.None;

        public override int GetHashCode()
        {
            return (int)Tale1 | (int)Tale2 | (int)Tale3;
        }

        public override bool Equals(object? obj) => Equals(obj as Piece);

        public bool Equals(Piece? other)
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

        public static bool operator == (Piece? piece1, Piece? piece2)
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

        public static bool operator != (Piece? piece1, Piece? piece2)
        {
            return !(piece1 == piece2);
        }
    }
}