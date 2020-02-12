using System;

using Xunit;

namespace RubiksCube.Test
{
    public class CubeBuilderTests
    {
        [Fact]
        public void CreateEmpty_GeneratesCubeWithCentralPiecesSet()
        {
            // Arrange & Act
            var builder = CubeBuilder.CreateEmpty();

            // Assert
            Assert.Equal(27 - 1 - 6, builder.EmptyPieceSpaces);
            
            Assert.Equal(new CentralPiece(Color.Red), builder.GetPiece((1, 1, 0)));
            Assert.Equal(new CentralPiece(Color.Orange), builder.GetPiece((1, 1, 2)));
            
            Assert.Equal(new CentralPiece(Color.White), builder.GetPiece((1, 2, 1)));
            Assert.Equal(new CentralPiece(Color.Yellow), builder.GetPiece((1, 0, 1)));

            Assert.Equal(new CentralPiece(Color.Blue), builder.GetPiece((2, 1, 1)));
            Assert.Equal(new CentralPiece(Color.Green), builder.GetPiece((0, 1, 1)));
        }

        [Fact]
        public void AddTwoCornerPiece_AddsThePieceToTheRightPlace()
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            var coordinate = new CubeCoordinates(1, 2, 0);

            // Act
            builder.AddPiece(new TwoCornerPiece(Color.White, Color.Red), coordinate);

            // Assert            
            Assert.Equal(27 - 1 - 6 - 1, builder.EmptyPieceSpaces);
            Assert.Equal(new TwoCornerPiece(Color.White, Color.Red), builder.GetPiece((1, 2, 0)));
        }

        [Theory]
        [InlineData(0, 0, 0)]   // three corner piece
        [InlineData(1, 1, 1)]   // center of the cube
        [InlineData(1, 1, 0)]   // center of a wall
        public void AddTwoCornerPiece_ThrowsArgumentExcepotion_WhenCoordinateIsInvalid(int x, int y, int z)
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            var coordinate = new CubeCoordinates(x, y, z);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => builder.AddPiece(new TwoCornerPiece(Color.White, Color.Red), coordinate));
        }

        [Fact]
        public void AddThreeCornerPiece_AddsThePieceToTheRightPlace()
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            var coordinate = new CubeCoordinates(0, 0, 0);

            // Act
            builder.AddPiece(new ThreeCornerPiece(Color.White, Color.Red, Color.Blue), coordinate);

            // Assert
            Assert.Equal(27 - 1 - 6 - 1, builder.EmptyPieceSpaces);
            Assert.Equal(new ThreeCornerPiece(Color.White, Color.Red, Color.Blue), builder.GetPiece((0, 0, 0)));
        }

        [Theory]
        [InlineData(0, 1, 0)] // two corner piece
        [InlineData(1, 1, 1)] // center of the cube
        [InlineData(1, 1, 0)] // center
        public void AddThreeCornerPiece_ThrowsArgumentExcepotion_WhenCoordinateIsInvalid(int x, int y, int z)
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            var coordinate = new CubeCoordinates(x, y, z);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => builder.AddPiece(new ThreeCornerPiece(Color.White, Color.Red, Color.Blue), coordinate));
        }
    }
}
