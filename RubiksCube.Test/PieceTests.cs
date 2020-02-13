using Xunit;

namespace RubiksCube.Test
{
    public class PieceTests
    {
        [Fact]
        public void CentralPiece_Works()
        {
            // Arrange & Act
            var piece = Piece.CentralPiece(Color.Blue);

            // Assert
            Assert.Equal(Color.Blue, piece.Tale1);
        }

        [Fact]
        public void GetHashCode_ReturnsColorsValue()
        {
            // Arrange
            var piece = Piece.CentralPiece(Color.Blue);

            // Act & Assert
            Assert.Equal((int)Color.Blue, piece.GetHashCode());
        }

        [Fact]
        public void Equals_ReturnsTrue_WhenComparedWithDifferentObjectWithSameValues()
        {
            // Arrange
            var piece1 = Piece.CentralPiece(Color.Blue);
            var piece2 = Piece.CentralPiece(Color.Blue);

            // Act & Assert
            Assert.True(piece1.Equals(piece2));
        }

        [Fact]
        public void Equals_ReturnsFalse_WhenComparedWithDifferentObjectWithDifferentValues()
        {
            // Arrange
            var piece1 = Piece.CentralPiece(Color.Blue);
            var piece2 = Piece.CentralPiece(Color.Red);

            // Act & Assert
            Assert.False(piece1.Equals(piece2));
        }
    }
}
