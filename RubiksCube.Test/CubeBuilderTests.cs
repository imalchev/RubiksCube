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
            
            Assert.Equal(Pieces.Red, builder.GetPiece((1, 1, 0)));
            Assert.Equal(Pieces.Orange, builder.GetPiece((1, 1, 2)));
            
            Assert.Equal(Pieces.White, builder.GetPiece((1, 2, 1)));
            Assert.Equal(Pieces.Yellow, builder.GetPiece((1, 0, 1)));

            Assert.Equal(Pieces.Blue, builder.GetPiece((2, 1, 1)));
            Assert.Equal(Pieces.Green, builder.GetPiece((0, 1, 1)));
        }

        [Fact]
        public void AddTwoCornerPiece_AddsThePieceToTheRightPlace()
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            var coordinate = new CubeCoordinates(1, 2, 0);

            // Act
            builder.AddPiece(coordinate, Pieces.RedWhite);

            // Assert            
            Assert.Equal(27 - 1 - 6 - 1, builder.EmptyPieceSpaces);
            Assert.Equal(Pieces.RedWhite, builder.GetPiece((1, 2, 0)));
        }

        [Theory]
        [InlineData(0, 0, 0)]   // three corner piece
        [InlineData(1, 1, 1)]   // center of the cube
        [InlineData(1, 1, 0)]   // center of a wall
        public void AddTwoCornerPiece_ThrowsArgumentException_WhenCoordinateIsInvalid(int x, int y, int z)
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            var coordinate = new CubeCoordinates(x, y, z);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => builder.AddPiece(coordinate, Pieces.RedWhite));
        }

        [Fact]
        public void AddThreeCornerPiece_AddsThePieceToTheRightPlace()
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            var coordinate = new CubeCoordinates(0, 0, 0);

            // Act
            builder.AddPiece(coordinate, Pieces.RedBlueWhite);

            // Assert
            Assert.Equal(27 - 1 - 6 - 1, builder.EmptyPieceSpaces);
            Assert.Equal(Pieces.RedBlueWhite, builder.GetPiece((0, 0, 0)));
        }

        [Theory]
        [InlineData(0, 1, 0)] // two corner piece
        [InlineData(1, 1, 1)] // center of the cube
        [InlineData(1, 1, 0)] // center
        public void AddThreeCornerPiece_ThrowsArgumentException_WhenCoordinateIsInvalid(int x, int y, int z)
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            var coordinate = new CubeCoordinates(x, y, z);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => builder.AddPiece(coordinate, Pieces.RedBlueWhite));
        }

        [Fact]
        public void Build_ShouldBuildCubeSuccessfully()
        {
            // Arrange
            var builder = CubeBuilder.CreateEmpty();
            
            builder.AddPiece((0, 0, 0), Pieces.YellowRedGreen);
            builder.AddPiece((1, 0, 0), Pieces.YellowRed);
            builder.AddPiece((2, 0, 0), Pieces.YellowRedBlue);
            builder.AddPiece((0, 1, 0), Pieces.RedGreen);
            builder.AddPiece((2, 1, 0), Pieces.RedBlue);
            builder.AddPiece((0, 2, 0), Pieces.RedGreenWhite);
            builder.AddPiece((1, 2, 0), Pieces.RedWhite);
            builder.AddPiece((2, 2, 0), Pieces.RedBlueWhite);
            
            builder.AddPiece((0, 0, 1), Pieces.YellowGreen);
            builder.AddPiece((2, 0, 1), Pieces.YellowBlue);
            builder.AddPiece((0, 0, 2), Pieces.YellowGreenOrange);
            builder.AddPiece((1, 0, 2), Pieces.YellowOrange);
            builder.AddPiece((1, 2, 2), Pieces.OrangeWhite);
            builder.AddPiece((2, 0, 2), Pieces.YellowBlueOrange);
            
            builder.AddPiece((0, 1, 2), Pieces.GreenOrange);
            builder.AddPiece((0, 2, 2), Pieces.GreenOrangeWhite);
            builder.AddPiece((0, 2, 1), Pieces.GreenWhite);

            builder.AddPiece((2, 2, 1), Pieces.BlueWhite);
            builder.AddPiece((2, 2, 2), Pieces.BlueOrangeWhite);
            builder.AddPiece((2, 1, 2), Pieces.BlueOrange);

            Cube cube = builder.Build();
        }
    }
}
