using System;

namespace RubiksCube
{
    public enum RotationSegment
    {
        First = 0,
        Third = 2
    }

    public class RotationCommand
    {
        public Orientation Orientation { get; }
        public RotationSegment Segment { get; }
        public int Turnover { get; }

        public RotationCommand(Orientation orientation, RotationSegment segment, int turnover = 1)
        {
            Orientation = orientation;
            Segment = segment;
            Turnover = turnover;
        }
    }

    public class Cube
    {
        private Piece[,,] _state = new Piece[3, 3, 3];

        internal Cube(Piece[,,] state)
        {
            _state = CopyState(state);
        }

        public Cube Rotate(RotationCommand command)
        {
            var newState = CopyState(_state);



            return new Cube(newState);
        }

        public Color[,] GetSideColors(Color centerColor)
        {
            return new Color[3, 3];
        }

        public Color[,] GetSideColors(CubeCoordinates centerCoordinate)
        {
            if (!centerCoordinate.HasOneOuterTale)
            {
                throw new ArgumentException(
                    $"The {nameof(centerCoordinate)} should represent the center of the side of the cube!", 
                    nameof(centerCoordinate));
            }

            var result = new Color[3, 3];
            
            for (int index1 = 0; index1 < 3; index1++)
            {
                for (int index2 = 0; index2 < 3; index2++)
                {
                    if (centerCoordinate.X == 0 || centerCoordinate.X == 2)
                    {
                        result[index1, index2] = _state[centerCoordinate.X, index1, index2].Tale1;
                    }
                    else if(centerCoordinate.Y == 0 || centerCoordinate.Y == 2)
                    {
                        result[index1, index2] = _state[index1, centerCoordinate.Y, index2].Tale2;
                    }
                    else if(centerCoordinate.Z == 0 || centerCoordinate.Z == 2)
                    {
                        result[index1, index2] = _state[index1, index2, centerCoordinate.Z].Tale3;
                    }
                }
            }

            return result;
        }

        private Piece[,,] CopyState(Piece[,,] state)
        {
            var newState = new Piece[3, 3, 3];
            for (int indexX = 0; indexX < 3; indexX++)            
            {
                for (int indexY = 0; indexY < 3; indexY++)
                {
                    for (int indexZ = 0; indexZ < 3; indexZ++)
                    {
                        newState[indexX, indexY, indexZ] = state[indexX, indexY, indexZ];
                    }
                }
            }

            return newState;
        }
    }
}
