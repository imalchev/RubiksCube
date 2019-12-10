using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

    public class CentarPiece : IPice, IEquatable<CentarPiece>
    {
        public Color Tale1 { get; }

        public CentarPiece(Color color)
        {
            Tale1 = color;
        }

        public override int GetHashCode()
        {
            return (int)Tale1;
        }

        public override bool Equals(object? obj) => Equals(obj as CentarPiece);

        public bool Equals([AllowNull] CentarPiece? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Tale1 == other.Tale1;
        }
    }

    public class TwoCorrnerPiece : CentarPiece, IEquatable<TwoCorrnerPiece>
    {        
        public Color Tale2 { get; }
    
        public TwoCorrnerPiece(Color color1, Color color2) 
            : base(color1)
        {
            if (color1 == color2)
            {
                throw new ArgumentException("The colors can't be equal!",nameof(color2));
            }

            Tale2 = color2;
        }

        public override int GetHashCode()
        {
            return (int)Tale1 | (int)Tale2;
        }

        public override bool Equals(object? obj) => Equals(obj as TwoCorrnerPiece);

        public bool Equals([AllowNull] TwoCorrnerPiece? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ((int)Tale1 | (int)Tale2) == ((int)other.Tale1 | (int)other.Tale2);
        }
    }

    public class ThreeCorrnerPiece : TwoCorrnerPiece, IEquatable<ThreeCorrnerPiece>
    {
        public Color Tale3 { get; }

        public ThreeCorrnerPiece(Color color1, Color color2, Color color3)
            : base(color1, color2)
        {
            if (color1 == color3 || color2 == color3)
            {
                throw new ArgumentException("The colors can't be equal!", nameof(color3));
            }

            Tale3 = color3;
        }

        public override int GetHashCode()
        {
            return (int)Tale1 | (int)Tale2 | (int)Tale3;
        }

        public bool Equals([AllowNull] ThreeCorrnerPiece other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ((int)Tale1 | (int)Tale2 | (int)Tale3) == ((int)other.Tale1 | (int)other.Tale2 | (int)other.Tale3);
        }
    }

    public class Cube
    {
        private static HashSet<IPice> s_pieces = new HashSet<IPice> 
        {
            new CentarPiece(Color.Yellow),   
            new CentarPiece(Color.Red),
            new CentarPiece(Color.Blue),
            new CentarPiece(Color.Green),
            new CentarPiece(Color.White),
            new CentarPiece(Color.Orange),

            new TwoCorrnerPiece(Color.Yellow, Color.Red), 
            new TwoCorrnerPiece(Color.Yellow, Color.Green),
            new TwoCorrnerPiece(Color.Yellow, Color.Blue),
            new TwoCorrnerPiece(Color.Yellow, Color.Orange),
            new TwoCorrnerPiece(Color.Red, Color.Green),
            new TwoCorrnerPiece(Color.Red, Color.Blue),
            new TwoCorrnerPiece(Color.Red, Color.White),
            new TwoCorrnerPiece(Color.Blue, Color.White),
            new TwoCorrnerPiece(Color.Blue, Color.Orange),            
            new TwoCorrnerPiece(Color.Green, Color.Orange),
            new TwoCorrnerPiece(Color.Green, Color.White),
            new TwoCorrnerPiece(Color.Orange, Color.White),

            new ThreeCorrnerPiece(Color.Yellow, Color.Red, Color.Blue),
            new ThreeCorrnerPiece(Color.Yellow, Color.Red, Color.Green),
            new ThreeCorrnerPiece(Color.Yellow, Color.Green, Color.Orange),
            new ThreeCorrnerPiece(Color.Yellow, Color.Blue, Color.Orange),
            new ThreeCorrnerPiece(Color.Red, Color.Green, Color.White),
            new ThreeCorrnerPiece(Color.Red, Color.Blue, Color.White),
            new ThreeCorrnerPiece(Color.Blue, Color.Orange, Color.White),
            new ThreeCorrnerPiece(Color.Green, Color.Orange, Color.White)
        };

        private CentarPiece Yellow { get; } = new CentarPiece(Color.Yellow);

        private IPice[,,] _state = new IPice[3, 3, 3];
    }
}