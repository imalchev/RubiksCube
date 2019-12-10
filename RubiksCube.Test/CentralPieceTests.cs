using RubiksCube;
using Xunit;

namespace RubiksCube.Test
{
    public class CentralPieceTests
    {
        [Fact]
        public void Ctor_Works()
        {
            // Arrange & Act
            var piece = new CentarPiece(Color.Blue);

            // Assert
            Assert.Equal(Color.Blue, piece.Tale1);
        }

        [Fact]
        public void GetHashCode_Returns_ColorsValue()
        {
            // Arrange
            var piece = new CentarPiece(Color.Blue);

            // Act & Assert
            Assert.Equal((int)Color.Blue, piece.GetHashCode());
        }
    }
}