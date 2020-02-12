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
        private IPiece[,,] _state = new IPiece[3, 3, 3];

        internal Cube(IPiece[,,] state)
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
            if (centerCoordinate.X == 0 || centerCoordinate.X == 2)
            {
                for (int index1 = 0; index1 < 3; index1++)            
                {
                    for (int index2 = 0; index2 < 3; index2++)
                    {                        
                        result[index1, index2] = _state[centerCoordinate.X, index1, index2].GetColor(Orientation.X);
                    }
                }
            }

            return result;
        }

        private IPiece[,,] CopyState(IPiece[,,] state)
        {
            var newState = new IPiece[3, 3, 3];
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
