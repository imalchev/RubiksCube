using System;
using System.Collections.Generic;
using System.Linq;

namespace RubiksCube
{    
    public class Cube
    {
        private CubePiece[,,] _state; // = new CubePiece[3, 3, 3];

        internal Cube(CubePiece[,,] state)
        {
            _state = state; // CopyState(state);
        }

        public Cube Rotate(RotationCommand command)
        {
            var newState = CopyState(_state);

            if (command.RotationAxis == Orientation.YellowWhite)
            {
                var y = (int)command.Segment; // 0 or 2

                foreach (var indexes in GetRotationIndexes())
                {
                    (int index1, int index2) = RotateIndexes((indexes.Index1, indexes.Index2), command.Turnovers);

                    newState[index1, y, index2] = RotatePiece((indexes.Index1, y, indexes.Index2), command.RotationAxis, command.Turnovers);
                }
            }
            else if (command.RotationAxis == Orientation.GreenBlue)
            {
                 var x = (int)command.Segment; // 0 or 2

                foreach (var indexes in GetRotationIndexes())
                {
                    (int index1, int index2) = RotateIndexes((indexes.Index1, indexes.Index2), command.Turnovers);

                    newState[x, index1, index2] = RotatePiece((x, indexes.Index1, indexes.Index2), command.RotationAxis, command.Turnovers);
                }
            }
            else if (command.RotationAxis == Orientation.RedOrange)
            {
                var z = (int)command.Segment; // 0 or 2

                foreach (var indexes in GetRotationIndexes())
                {
                    (int index1, int index2) = RotateIndexes((indexes.Index1, indexes.Index2), command.Turnovers);

                    newState[index1, index2, z] = RotatePiece((indexes.Index1, indexes.Index2, z), command.RotationAxis, command.Turnovers);
                }
            }

            return new Cube(newState);
        }

        /// <summary>
        /// Gets enumerable of the source indexes that change when rotating a cube side.
        /// </summary>
        private IEnumerable<(int Index1, int Index2)> GetRotationIndexes()
        {
            yield return (0, 0);
            yield return (0, 1);
            yield return (0, 2);

            yield return (1, 0);
            yield return (1, 2);

            yield return (2, 0);
            yield return (2, 1);
            yield return (2, 2);
        }

        /// <summary>
        /// Gets a tuple of cube indexes that change on rotation of the cube base on turnovers count.
        /// </summary>
        private (int, int) RotateIndexes((int index1, int index2) indexes, int turnovers)
        {
            if (turnovers < 0 || turnovers >= 4)
            {
                throw new ArgumentOutOfRangeException(nameof(turnovers));
            }

            IEnumerable<(int Index1, int Index2)> GetThreeSideEdgesRotationRow()
            {
                // (0, 0) -> (2, 0) -> (2, 2) -> (0, 2) -> repeat                
                yield return (0, 0);
                yield return (2, 0);
                yield return (2, 2);
                yield return (0, 2);

                yield return (0, 0);
                yield return (2, 0);
                yield return (2, 2);
                yield return (0, 2);
            }

            IEnumerable<(int Index1, int Index2)> GetTwoSideEdgesRotationRow()
            {
                // (1, 0) -> (2, 1) -> (1, 2) -> (0, 1) -> repeat ...
                yield return (1, 0);
                yield return (2, 1);
                yield return (1, 2);
                yield return (0, 1);

                yield return (1, 0);
                yield return (2, 1);
                yield return (1, 2);
                yield return (0, 1);
            }

            IEnumerable<(int, int)> threeSideEdgesRow = GetThreeSideEdgesRotationRow()
                .SkipWhile(x => (x.Index1 == indexes.index1 && x.Index2 == indexes.index2) == false);

            if (threeSideEdgesRow.Any())
            {
                return threeSideEdgesRow.Skip(turnovers).First();
            }

            IEnumerable<(int, int)> twoSideEdgesRow = GetTwoSideEdgesRotationRow()
                .SkipWhile(x => (x.Index1 == indexes.index1 && x.Index2 == indexes.index2) == false);

            if (twoSideEdgesRow.Any())
            {
                return twoSideEdgesRow.Skip(turnovers).First();
            }

            throw new InvalidOperationException($"Can't find indexes for ({indexes.index1}, {indexes.index2})!");
        }

        private CubePiece RotatePiece(CubeCoordinates source, Orientation rotationAsses, int turnovers)
        {
            var sourceCubePiece = _state[source.X, source.Y, source.Z];
            
            var result = new CubePiece(
                sourceCubePiece.Piece,
                RotateTale(sourceCubePiece.Tale1Orientation, rotationAsses, turnovers),
                RotateTale(sourceCubePiece.Tale2Orientation, rotationAsses, turnovers),
                RotateTale(sourceCubePiece.Tale3Orientation, rotationAsses, turnovers));

            return result;
        }

        private Orientation RotateTale(Orientation sourceTale, Orientation rotationAsses, int turnovers)
        {
            if (sourceTale == rotationAsses || sourceTale == Orientation.None)
            {
                // stays the same
                return sourceTale;
            }

            // 2, 4, 6, ... 2n rotations put the tale in same asses as it is
            if (turnovers % 2 == 0)
            {
                return sourceTale;
            }

            if (rotationAsses == Orientation.RedOrange)
            {
                if (sourceTale == Orientation.GreenBlue)
                {                     
                    return Orientation.YellowWhite;
                }
                else
                {
                    return Orientation.GreenBlue;
                }
            }
            else if (rotationAsses == Orientation.GreenBlue)
            {
                if (sourceTale == Orientation.RedOrange)
                {
                    return Orientation.YellowWhite;
                }
                else
                {
                    return Orientation.RedOrange;
                }
            }
            else // rotationAsses == Orientation.YellowWhite
            {
                if (sourceTale == Orientation.RedOrange)
                {
                    return Orientation.GreenBlue;
                }
                else
                {
                    return Orientation.RedOrange;
                }
            }
        }
        
        public Color[,] GetSideColors(Color centerColor)
        {
            switch (centerColor)
            {
                case Color.Red: return GetSideColors((1, 1, 0));
                case Color.Orange: return GetSideColors((1, 1, 2));

                case Color.Green: return GetSideColors((0, 1, 1));
                case Color.Blue: return GetSideColors((2, 1, 1));

                case Color.Yellow: return GetSideColors((1, 0, 1));
                case Color.White: return GetSideColors((1, 2, 1));

                default: throw new InvalidOperationException($"Color {Enum.GetName(typeof(Color), centerColor)} is invalid!");
            }
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
                        result[index1, index2] = _state[centerCoordinate.X, index1, index2].GetColor(Orientation.GreenBlue);
                    }
                }
            }
            else if(centerCoordinate.Y == 0 || centerCoordinate.Y == 2)
            {
                for (int index1 = 0; index1 < 3; index1++)
                {
                    for (int index2 = 0; index2 < 3; index2++)
                    {
                        result[index1, index2] = _state[index1, centerCoordinate.Y, index2].GetColor(Orientation.YellowWhite);
                    }
                }
            }
            else if(centerCoordinate.Z == 0 || centerCoordinate.Z == 2)
            {
                for (int index1 = 0; index1 < 3; index1++)
                {
                    for (int index2 = 0; index2 < 3; index2++)
                    {
                        result[index1, index2] = _state[index1, index2, centerCoordinate.Z].GetColor(Orientation.RedOrange);
                    }
                }
            }            

            return result;
        }

        private CubePiece[,,] CopyState(CubePiece[,,] state)
        {
            var newState = new CubePiece[3, 3, 3];
            for (int indexX = 0; indexX < 3; indexX++)            
            {
                for (int indexY = 0; indexY < 3; indexY++)
                {
                    for (int indexZ = 0; indexZ < 3; indexZ++)
                    {
                        if (indexX == 1 && indexY == 1 && indexZ == 1)
                        {
                            continue;
                        }

                        newState[indexX, indexY, indexZ] = new CubePiece(
                            state[indexX, indexY, indexZ].Piece,
                            state[indexX, indexY, indexZ].Tale1Orientation,
                            state[indexX, indexY, indexZ].Tale2Orientation,
                            state[indexX, indexY, indexZ].Tale3Orientation);
                    }
                }
            }

            return newState;
        }
    }
}
