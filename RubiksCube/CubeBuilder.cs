using System;
using System.Collections.Generic;
using System.Linq;

namespace RubiksCube
{
    public class CubeBuilder
    {
        private readonly IPiece[,,] _state;

        public static CubeBuilder CreateEmpty()
        {
            return new CubeBuilder();
        }

        private CubeBuilder()
        {
            _state = new IPiece[3, 3, 3];

            // initialize centers of all the walls
            _state[1, 0, 1] = Pieces.Yellow;   // opposite to White 
            _state[1, 2, 1] = Pieces.White;    // opposite to Yellow

            _state[1, 1, 0] = Pieces.Red;      // opposite to Orange
            _state[1, 1, 2] = Pieces.Orange;   // opposite to Red

            _state[0, 1, 1] = Pieces.Green;    // opposite to Blue
            _state[2, 1, 1] = Pieces.Blue;     // opposite to Green
        }

        /// <summary>
        /// Adds a piece to the cube if it is not being added already.
        /// <summary>
        public CubeBuilder AddPiece(CubeCoordinates coordinate, TwoCornerPiece piece)
        {
            var pieceAlreadyInTheCube = _state.AsEnumerable().Any(x => x.Equals(piece));
            if (pieceAlreadyInTheCube)
            {
                throw new ArgumentException("This piece is in the cube already!", nameof(piece));
            }

            if (!coordinate.HasTwoOuterTales)
            {
                throw new ArgumentException(
                    $"The provided coordinate is not valid place for putting {nameof(TwoCornerPiece)}!",
                    nameof(coordinate));
            }

            _state[coordinate.X, coordinate.Y, coordinate.Z] = piece;

            return this;
        }

        /// <summary>
        /// Adds a piece to the cube if it is not being added already.
        /// <summary>
        public CubeBuilder AddPiece(CubeCoordinates coordinate, ThreeCornerPiece piece)
        {
            var pieceAlreadyInTheCube = _state.AsEnumerable().Any(x => x.Equals(piece));
            if (pieceAlreadyInTheCube)
            {
                throw new ArgumentException("This piece is in the cube already!", nameof(piece));
            }

            if (!coordinate.HasThreeOuterTales)
            {
                throw new ArgumentException(
                    $"The provided coordinate is not valid place for putting {nameof(ThreeCornerPiece)}!",
                    nameof(coordinate));
            }

            _state[coordinate.X, coordinate.Y, coordinate.Z] = piece;

            return this;
        }

        public IPiece GetPiece(CubeCoordinates coordinate) => _state[coordinate.X, coordinate.Y, coordinate.Z];
        

        /// <summary>
        /// Returns the count of empty piece spaces in the cube.
        /// </summary>
        public int EmptyPieceSpaces =>
            // the cube consists of 3 * 3 * 3 = 27 piece spaces - the center space is not visible
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
                throw new InvalidOperationException("The cube is not fully populated. It can't be build!");
            }

            return new Cube(_state);
        }
    }
}
