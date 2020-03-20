using System;
using RubiksCube;
using Xunit;

namespace RubiksCube.Test
{
    public class CubeTests
    {
        [Fact]
        public void GetSideColors_Works()
        {
            // Arrange
            var orderedCube = CubeBuilder.CreateOrdered().Build();

            // Act
            var redSideColors = orderedCube.GetSideColors((1, 1, 0)); // gets red side

            // Assert
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.Equal(Color.Red, redSideColors[i, j]);
                }
            }
        }

        [Fact]
        public void RotateOnYellowWhiteOnFirstRowWithOneTurnover_Works()
        {
            // Arrange
            var orderedCube = CubeBuilder.CreateOrdered().Build();

            var command = new RotationCommand(Orientation.YellowWhite, RotationSegment.First, turnovers: 1);

            // Act
            var resltedCube = orderedCube.Rotate(command);

            // Assert

            #region testing white side
            // white side should stay the same
            var whiteSideColors = resltedCube.GetSideColors(Color.White);
            
            // first row
            Assert.Equal(Color.White, whiteSideColors[0, 2]);
            Assert.Equal(Color.White, whiteSideColors[1, 2]);
            Assert.Equal(Color.White, whiteSideColors[2, 2]);

            // second row
            Assert.Equal(Color.White, whiteSideColors[0, 1]);
            Assert.Equal(Color.White, whiteSideColors[1, 1]);
            Assert.Equal(Color.White, whiteSideColors[2, 1]);

            // third row
            Assert.Equal(Color.White, whiteSideColors[0, 0]);
            Assert.Equal(Color.White, whiteSideColors[1, 0]);
            Assert.Equal(Color.White, whiteSideColors[2, 0]);
            #endregion

            #region testing yellow side
            // white side should stay the same
            var yellowSideColors = resltedCube.GetSideColors(Color.Yellow);
            
            // first row
            Assert.Equal(Color.Yellow, yellowSideColors[0, 2]);
            Assert.Equal(Color.Yellow, yellowSideColors[1, 2]);
            Assert.Equal(Color.Yellow, yellowSideColors[2, 2]);

            // second row
            Assert.Equal(Color.Yellow, yellowSideColors[0, 1]);
            Assert.Equal(Color.Yellow, yellowSideColors[1, 1]);
            Assert.Equal(Color.Yellow, yellowSideColors[2, 1]);

            // third row
            Assert.Equal(Color.Yellow, yellowSideColors[0, 0]);
            Assert.Equal(Color.Yellow, yellowSideColors[1, 0]);
            Assert.Equal(Color.Yellow, yellowSideColors[2, 0]);
            #endregion

            #region testing red side
            var redSideColors = resltedCube.GetSideColors(Color.Red);

            // first column
            Assert.Equal(Color.Green, redSideColors[0, 0]);
            Assert.Equal(Color.Red, redSideColors[0, 1]);
            Assert.Equal(Color.Red, redSideColors[0, 2]);

            // second column
            Assert.Equal(Color.Green, redSideColors[1, 0]);
            Assert.Equal(Color.Red, redSideColors[1, 1]);
            Assert.Equal(Color.Red, redSideColors[1, 2]);

            // third column
            Assert.Equal(Color.Green, redSideColors[2, 0]);
            Assert.Equal(Color.Red, redSideColors[2, 1]);
            Assert.Equal(Color.Red, redSideColors[2, 2]);
            #endregion

            #region testing green side
            var greenSideColors = resltedCube.GetSideColors(Color.Green);
            
            // first row
            Assert.Equal(Color.Orange, greenSideColors[0, 0]);
            Assert.Equal(Color.Orange, greenSideColors[0, 1]);
            Assert.Equal(Color.Orange, greenSideColors[0, 2]);

            // second row
            Assert.Equal(Color.Green, greenSideColors[1, 0]);
            Assert.Equal(Color.Green, greenSideColors[1, 1]);
            Assert.Equal(Color.Green, greenSideColors[1, 2]);

            // third row
            Assert.Equal(Color.Green, greenSideColors[2, 0]);
            Assert.Equal(Color.Green, greenSideColors[2, 1]);
            Assert.Equal(Color.Green, greenSideColors[2, 2]);
            #endregion

            #region testing orange side
            var orangeSideColors = resltedCube.GetSideColors(Color.Orange);
            
            // first row
            Assert.Equal(Color.Blue, orangeSideColors[0, 0]);
            Assert.Equal(Color.Blue, orangeSideColors[1, 0]);
            Assert.Equal(Color.Blue, orangeSideColors[2, 0]);

            // second row
            Assert.Equal(Color.Orange, orangeSideColors[0, 1]);
            Assert.Equal(Color.Orange, orangeSideColors[1, 1]);
            Assert.Equal(Color.Orange, orangeSideColors[2, 1]);

            // third row
            Assert.Equal(Color.Orange, orangeSideColors[0, 2]);
            Assert.Equal(Color.Orange, orangeSideColors[1, 2]);
            Assert.Equal(Color.Orange, orangeSideColors[2, 2]);
            #endregion
        }
    }
}
