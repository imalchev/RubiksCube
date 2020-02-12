using System.Collections.Generic;

namespace RubiksCube
{
    public static class Extensions
    {
        /// <summary>
        /// Enumerates all the populated visible pieces of the cube.
        /// <summary>
        public static IEnumerable<IPiece> AsEnumerable(this IPiece[,,] pieces)
        {
            for (int indexX = 0; indexX < 3; indexX++)
            {
                for (int indexY = 0; indexY < 3; indexY++)
                {
                    for (int indexZ = 0; indexZ < 3; indexZ++)
                    {
                        // skip the center of the cube
                        if (indexX == 1 && indexY == 1 && indexZ == 1)
                        {   
                            continue;
                        }

                        if (pieces[indexX, indexY, indexZ] != null)
                        {
                            yield return pieces[indexX, indexY, indexZ];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates all empty (non-populated) visible pieces of the cube.
        /// <summary>
        public static IEnumerable<CubeCoordinates> EnumerateEmptySpaces(this IPiece[,,] pieces)
        {
            for (int indexX = 0; indexX < 3; indexX++)
            {
                for (int indexY = 0; indexY < 3; indexY++)
                {
                    for (int indexZ = 0; indexZ < 3; indexZ++)
                    {
                        // skip the center of the cube
                        if (indexX == 1 && indexY == 1 && indexZ == 1)
                        {   
                            continue;
                        }

                        if (pieces[indexX, indexY, indexZ] == null)
                        {
                            yield return new CubeCoordinates(indexX, indexY, indexZ);
                        }
                    }
                }
            }
        }
    }
}