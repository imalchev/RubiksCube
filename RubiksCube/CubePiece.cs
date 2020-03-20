namespace RubiksCube
{    
    public class CubePiece
    {
        public Piece Piece { get; }
        public Orientation Tale1Orientation { get; }
        public Orientation Tale2Orientation { get; }
        public Orientation Tale3Orientation { get; }

        public CubePiece(Piece piece, 
            Orientation tale1Orientation, 
            Orientation tale2Orientation = Orientation.None, 
            Orientation tale3Orientation = Orientation.None)
        {
            Piece = piece;
            Tale1Orientation = tale1Orientation;
            Tale2Orientation = tale2Orientation;
            Tale3Orientation = tale3Orientation;
        }

        /// <summary>
        /// Gets color of the piece base on the requested piece orientation
        /// </summary>
        public Color GetColor(Orientation orientation)
        {
            if (Tale1Orientation == orientation)
            {
                return Piece.Tale1;
            }

            if (Tale2Orientation == orientation)
            {
                return Piece.Tale2;
            }

            if (Tale3Orientation == orientation)
            {
                return Piece.Tale3;
            }

            return Color.None;
        }
    }
}