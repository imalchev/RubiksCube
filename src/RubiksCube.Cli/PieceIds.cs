using System.Linq;
using RubiksCube;

namespace RubiksCube.Cli
{
    /// <summary>
    /// A stable, orientation-independent identifier for a physical <see cref="Piece" />: its own
    /// colors (there's no inherent name/number on <see cref="Piece" /> itself), in a fixed canonical
    /// order, so the same piece always prints the same id no matter which of its tales is currently
    /// facing which axis. Centers get a 1-letter id (their only color), edges 2, corners 3 - e.g. the
    /// corner made of green/yellow/red tales is always "YGR", wherever it currently sits.
    /// </summary>
    public static class PieceIds
    {
        public static string GetId(Piece piece)
        {
            return string.Concat(
                new[] { piece.Tale1, piece.Tale2, piece.Tale3 }
                    .Where(color => color != Color.None)
                    .OrderBy(color => (int)color)
                    .Select(CubeColors.Abbreviate));
        }
    }
}
