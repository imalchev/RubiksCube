using System;

using Xunit;

namespace RubiksCube.Test
{
    public class CubeCoordinatesTests
    {
        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(1, 1, 2)]
        [InlineData(0, 0, 2)]
        public void Ctor_Works(int x, int y, int z)
        {
            // Arrange & Act
            var coordinate = new CubeCoordinates(x, y, z);

            // Assert
            Assert.Equal(x, coordinate.X);
            Assert.Equal(y, coordinate.Y);
            Assert.Equal(z, coordinate.Z);
        }

        [Theory]
        [InlineData(1, 0, 3)]
        [InlineData(1, -1, 2)]
        [InlineData(-1, 2, 1)]
        public void Ctor_Should_ThrowArgumentOutOfRangeException(int x, int y, int z)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CubeCoordinates(x, y, z));
        }

        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(1, 1, 2)]
        [InlineData(0, 0, 2)]
        public void ImplicitCast_Works(int x, int y, int z)
        {
            // Arrange & Act
            CubeCoordinates coordinate = (x, y, z);

            // Assert
            Assert.Equal(x, coordinate.X);
            Assert.Equal(y, coordinate.Y);
            Assert.Equal(z, coordinate.Z);
        }

        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(2, 0, 1)]
        [InlineData(1, 0, 2)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(2, 1, 0)]
        [InlineData(2, 1, 2)]
        public void IsTwoColors_Should_ReturnTrue(int x, int y, int z)
        {
            CubeCoordinates coordinate = (x, y, z);

            Assert.True(coordinate.IsTwoColors);
        }

        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(2, 2, 2)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 0, 2)]
        public void IsTwoColors_Should_ReturnFalse(int x, int y, int z)
        {
            CubeCoordinates coordinate = (x, y, z);

            Assert.False(coordinate.IsTwoColors);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(2, 0, 0)]
        [InlineData(2, 0, 2)]
        [InlineData(0, 0, 2)]
        [InlineData(0, 2, 0)]
        public void IsThreeColors_Should_ReturnTrue(int x, int y, int z)
        {
            CubeCoordinates coordinate = (x, y, z);

            Assert.True(coordinate.IsThreeColors);
        }

        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(1, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 1, 1)]
        public void IsThreeColors_Should_ReturnFalse(int x, int y, int z)
        {
            CubeCoordinates coordinate = (x, y, z);

            Assert.False(coordinate.IsThreeColors);
        }

        [Fact]
        public void Equals_ShouldBeTrue_WhenComparingDifferentObjectsWithSameValues()
        {
            // Arrange
            var firstCoordinate = new CubeCoordinates(1, 2, 0);
            var secondCoordinate = new CubeCoordinates(1, 2, 0);

            // Act & Assert
            Assert.True(firstCoordinate.Equals(secondCoordinate));
        }

         [Fact]
        public void Equals_Should_BeFalse_WhenComparingDifferentObjectsWithDifferentValues()
        {
            // Arrange
            var firstCoordinate = new CubeCoordinates(1, 2, 0);
            var secondCoordinate = new CubeCoordinates(1, 1, 0);

            // Act & Assert
            Assert.False(firstCoordinate.Equals(secondCoordinate));
        }
    }
}