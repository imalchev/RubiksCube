using RubiksCube;

namespace RubiksCube.Cli
{
    /// <summary>
    /// Shared color formatting used by both the per-tile display (<see cref="CubeFaceRenderer" />)
    /// and the piece identity used to track a physical piece across rotations (<see cref="PieceIds" />).
    /// </summary>
    internal static class CubeColors
    {
        public static char Abbreviate(Color color) => color switch
        {
            Color.White => 'W',
            Color.Yellow => 'Y',
            Color.Red => 'R',
            Color.Orange => 'O',
            Color.Green => 'G',
            Color.Blue => 'B',
            _ => '?',
        };
    }
}
