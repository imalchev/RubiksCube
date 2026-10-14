using RubiksCube;
using RubiksCube.Cli;
using Xunit;

namespace RubiksCube.Cli.Test
{
    public class PieceIdsTests
    {
        [Fact]
        public void GetId_CenterPiece_IsItsSingleColorLetter()
        {
            Assert.Equal("Y", PieceIds.GetId(Pieces.Yellow));
            Assert.Equal("R", PieceIds.GetId(Pieces.Red));
        }

        [Fact]
        public void GetId_EdgePiece_IsBothColorLettersInCanonicalOrder()
        {
            // White(1) < Yellow(2) < Green(4) < Blue(8) < Red(16) < Orange(32), regardless of
            // the order the piece's own Tale1/Tale2 were constructed in.
            Assert.Equal("YR", PieceIds.GetId(Pieces.YellowRed));
            Assert.Equal("WR", PieceIds.GetId(Pieces.RedWhite));
        }

        [Fact]
        public void GetId_CornerPiece_IsAllThreeColorLettersInCanonicalOrder()
        {
            Assert.Equal("YGR", PieceIds.GetId(Pieces.YellowRedGreen));
            Assert.Equal("YBO", PieceIds.GetId(Pieces.YellowBlueOrange));
        }

        [Fact]
        public void GetId_IsTheSameRegardlessOfOrientation()
        {
            // Pieces.All only has one instance per physical piece, but GetId must not depend on
            // which tale happens to be Tale1/2/3 - it's derived purely from the set of colors.
            var direct = PieceIds.GetId(Pieces.YellowRedGreen);

            var reoriented = new CubePiece(Pieces.YellowRedGreen, Orientation.GreenBlue, Orientation.YellowWhite, Orientation.RedOrange);

            Assert.Equal(direct, PieceIds.GetId(reoriented.Piece));
        }
    }
}
