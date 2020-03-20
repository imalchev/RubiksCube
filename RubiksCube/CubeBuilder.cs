using System;
using System.Collections.Generic;
using System.Linq;

namespace RubiksCube
{
    public class CubeBuilder
    {
        private readonly CubePiece[,,] _state;

        /// <summary>
        /// Creates empty cube builder. Next you have to fulfill all the cube pieces in order to build it.
        /// </summary>
        public static CubeBuilder CreateEmpty()
        {
            return new CubeBuilder();
        }

        public static CubeBuilder CreateOrdered()
        {
            var builder = new CubeBuilder();

            builder.AddPiece((0, 0, 0), Pieces.YellowRedGreen, Orientation.YellowWhite, Orientation.RedOrange, Orientation.GreenBlue);
            builder.AddPiece((1, 0, 0), Pieces.YellowRed, Orientation.YellowWhite, Orientation.RedOrange);
            builder.AddPiece((2, 0, 0), Pieces.YellowRedBlue, Orientation.YellowWhite, Orientation.RedOrange, Orientation.GreenBlue);
            builder.AddPiece((0, 1, 0), Pieces.RedGreen, Orientation.RedOrange, Orientation.GreenBlue);
            builder.AddPiece((2, 1, 0), Pieces.RedBlue, Orientation.RedOrange, Orientation.GreenBlue);
            builder.AddPiece((0, 2, 0), Pieces.RedGreenWhite, Orientation.RedOrange, Orientation.GreenBlue, Orientation.YellowWhite);
            builder.AddPiece((1, 2, 0), Pieces.RedWhite, Orientation.RedOrange, Orientation.YellowWhite);
            builder.AddPiece((2, 2, 0), Pieces.RedBlueWhite, Orientation.RedOrange, Orientation.GreenBlue, Orientation.YellowWhite);
            
            builder.AddPiece((0, 0, 1), Pieces.YellowGreen, Orientation.YellowWhite, Orientation.GreenBlue);
            builder.AddPiece((2, 0, 1), Pieces.YellowBlue, Orientation.YellowWhite, Orientation.GreenBlue);
            builder.AddPiece((0, 0, 2), Pieces.YellowGreenOrange, Orientation.YellowWhite, Orientation.GreenBlue, Orientation.RedOrange);
            builder.AddPiece((1, 0, 2), Pieces.YellowOrange, Orientation.YellowWhite, Orientation.RedOrange);
            builder.AddPiece((1, 2, 2), Pieces.OrangeWhite, Orientation.RedOrange, Orientation.YellowWhite);
            builder.AddPiece((2, 0, 2), Pieces.YellowBlueOrange, Orientation.YellowWhite, Orientation.GreenBlue, Orientation.RedOrange);
            
            builder.AddPiece((0, 1, 2), Pieces.GreenOrange, Orientation.GreenBlue, Orientation.RedOrange);
            builder.AddPiece((0, 2, 2), Pieces.GreenOrangeWhite, Orientation.GreenBlue, Orientation.RedOrange, Orientation.YellowWhite);
            builder.AddPiece((0, 2, 1), Pieces.GreenWhite, Orientation.GreenBlue, Orientation.YellowWhite);

            builder.AddPiece((2, 2, 1), Pieces.BlueWhite, Orientation.GreenBlue, Orientation.YellowWhite);
            builder.AddPiece((2, 2, 2), Pieces.BlueOrangeWhite, Orientation.GreenBlue, Orientation.RedOrange, Orientation.YellowWhite);
            builder.AddPiece((2, 1, 2), Pieces.BlueOrange, Orientation.GreenBlue, Orientation.RedOrange);

            return builder;
        }

        private CubeBuilder()
        {
            _state = new CubePiece[3, 3, 3];

            // initialize centers of all the walls
            _state[1, 0, 1] = new CubePiece(Pieces.Yellow, Orientation.YellowWhite);    // opposite to White 
            _state[1, 2, 1] = new CubePiece(Pieces.White, Orientation.YellowWhite);     // opposite to Yellow

            _state[1, 1, 0] = new CubePiece(Pieces.Red, Orientation.RedOrange);         // opposite to Orange
            _state[1, 1, 2] = new CubePiece(Pieces.Orange, Orientation.RedOrange);      // opposite to Red

            _state[0, 1, 1] = new CubePiece(Pieces.Green, Orientation.GreenBlue);       // opposite to Blue
            _state[2, 1, 1] = new CubePiece(Pieces.Blue, Orientation.GreenBlue);        // opposite to Green
        }

        /// <summary>
        /// Adds a piece to the cube if it is not being added already.
        /// <summary>
        /// <remarks>
        /// The piece is added in certain orientation. The orientation of the piece talse is defined by orientation parameters.
        /// </remarks>
        public CubeBuilder AddPiece(
            CubeCoordinates coordinate, 
            Piece piece, 
            Orientation tale1Orientation = Orientation.RedOrange,
            Orientation tale2Orientation = Orientation.YellowWhite,
            Orientation tale3Orientation = Orientation.None)
        {            
            if (coordinate.HasOneOuterTale || piece.HasOneOuterTale)
            {
                throw new ArgumentException(
                    "The provided coordinate or piece is not valid place for putting in the cube! " +
                    "It represents a central coordinate! The central coordinates are preset by the builder!");
            }

            if (tale1Orientation == Orientation.None)
            {
                throw new ArgumentException(
                    $"The provided tale 1 orientations is not valid for putting {nameof(Piece)}!",
                    nameof(tale1Orientation));
            }

            var pieceAlreadyInTheCube = _state.AsEnumerable().Any(x => x.Piece.Equals(piece));
            if (pieceAlreadyInTheCube)
            {
                throw new ArgumentException("This piece is in the cube already!", nameof(piece));
            }

            if (piece.HasTwoOuterTales && !coordinate.HasTwoOuterTales)
            {
                throw new ArgumentException(
                    $"The provided coordinate is not valid place for putting {nameof(Piece)}!",
                    nameof(coordinate));
            }            

            if (piece.HasThreeOuterTales && !coordinate.HasThreeOuterTales)
            {
                throw new ArgumentException(
                    $"The provided coordinate is not valid place for putting {nameof(Piece)}!",
                    nameof(coordinate));
            }

            // validate
            if (tale2Orientation == Orientation.None || tale1Orientation == tale2Orientation)
            {
                throw new ArgumentException(
                    $"The provided {nameof(tale2Orientation)} is not valid for putting {nameof(Piece)}!",
                    nameof(tale2Orientation));
            }

            if (piece.HasThreeOuterTales)
            {
                if (tale3Orientation == Orientation.None || tale2Orientation == tale3Orientation || tale1Orientation == tale3Orientation)
                {
                    throw new ArgumentException(
                        $"The provided {nameof(tale3Orientation)} is not valid place for putting {nameof(Piece)}!",
                        nameof(tale3Orientation));
                }
            }

            _state[coordinate.X, coordinate.Y, coordinate.Z] = 
                new CubePiece(piece, tale1Orientation, tale2Orientation, tale3Orientation);

            return this;
        }

        public Piece? GetPiece(CubeCoordinates coordinate) => _state[coordinate.X, coordinate.Y, coordinate.Z]?.Piece;

        public CubePiece? GetCubePiece(CubeCoordinates coordinate) => _state[coordinate.X, coordinate.Y, coordinate.Z];

        /// <summary>
        /// Returns the count of empty piece spaces in the cube.
        /// </summary>
        public int EmptyPieceSpaces =>
            // the cube consists of 3 * 3 * 3 = 27 piece spaces - the center space is not visible => 26
            26 - _state.AsEnumerable().Count();

        /// <summary>
        /// Get all available empty space coordinates.
        /// </summary>
        public IEnumerable<CubeCoordinates> GetEmptyPlaces() => _state.EnumerateEmptySpaces();

        /// <summary>
        /// Builds the cube only if all the pieces are set!
        /// </summary>
        public Cube Build()
        {
            if (EmptyPieceSpaces > 0)
            {
                var emptyPiece = GetEmptyPlaces().First();

                throw new InvalidOperationException("The cube is not fully populated. It can't be build! " +
                    $"One empty place is ({emptyPiece.X}, {emptyPiece.Y}, {emptyPiece.Z})!");
            }

            return new Cube(_state);
        }
    }
}
