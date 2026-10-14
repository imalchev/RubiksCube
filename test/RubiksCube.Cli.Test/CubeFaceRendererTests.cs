using System;
using System.IO;
using System.Linq;
using RubiksCube;
using RubiksCube.Cli;
using Xunit;

namespace RubiksCube.Cli.Test
{
    public class CubeFaceRendererTests
    {
        private static Cube Solved() => CubeBuilder.CreateOrdered().Build();

        [Fact]
        public void Render_PrintsTheCubeStateIdAboveTheMatrix()
        {
            var cube = Solved();
            var originalOut = Console.Out;
            string output;

            try
            {
                using var writer = new StringWriter();
                Console.SetOut(writer);
                CubeFaceRenderer.Render(cube);
                output = writer.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }

            var firstLine = output.Split('\n')[0].TrimEnd('\r');
            Assert.Equal($"State: {cube.GetStateId()}", firstLine);
        }

        [Theory]
        [InlineData(Color.Red)]
        [InlineData(Color.Orange)]
        [InlineData(Color.Yellow)]
        [InlineData(Color.White)]
        [InlineData(Color.Green)]
        [InlineData(Color.Blue)]
        public void BuildFaceGrid_OnASolvedCube_IsUniformlyTheFaceColor(Color face)
        {
            var grid = CubeFaceRenderer.BuildFaceGrid(Solved(), face);

            Assert.Equal(face, grid.CenterColor);

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Assert.Equal(face, grid.Cells[row, col].Color);
                }
            }
        }

        [Theory]
        [InlineData(Color.Red, 2)] // z == 0
        [InlineData(Color.Orange, 2)] // z == 2
        [InlineData(Color.Green, 0)] // x == 0
        [InlineData(Color.Blue, 0)] // x == 2
        [InlineData(Color.Yellow, 1)] // y == 0
        [InlineData(Color.White, 1)] // y == 2
        public void BuildFaceGrid_CoversExactlyTheCoordinatesOnThatFace(Color face, int fixedAxisIndex)
        {
            var grid = CubeFaceRenderer.BuildFaceGrid(Solved(), face);

            var coordinates = grid.Cells.Cast<Tile>().Select(tile => tile.Coordinate).ToList();
            Assert.Equal(9, coordinates.Distinct().Count());

            int Component(CubeCoordinates c) => fixedAxisIndex switch
            {
                0 => c.X,
                1 => c.Y,
                _ => c.Z,
            };

            var expectedValue = face == Color.Orange || face == Color.Blue || face == Color.White ? 2 : 0;

            Assert.All(coordinates, c => Assert.Equal(expectedValue, Component(c)));
        }

        [Fact]
        public void BuildNet_PlacesEachFaceAtItsLayoutOffset()
        {
            var net = CubeFaceRenderer.BuildNet(Solved());

            Assert.Equal(9, net.GetLength(0));
            Assert.Equal(12, net.GetLength(1));

            // White (Y=2, "up") is on top, Yellow (Y=0, "down") is on the bottom.
            AssertBlock(net, Color.White, 0, 1);
            AssertBlock(net, Color.Green, 1, 0);
            AssertBlock(net, Color.Red, 1, 1);
            AssertBlock(net, Color.Blue, 1, 2);
            AssertBlock(net, Color.Orange, 1, 3);
            AssertBlock(net, Color.Yellow, 2, 1);
        }

        [Fact]
        public void BuildNet_LeavesTheCrossCornersBlank()
        {
            var net = CubeFaceRenderer.BuildNet(Solved());

            foreach (var (rowBlock, colBlock) in new[] { (0, 0), (0, 2), (0, 3), (2, 0), (2, 2), (2, 3) })
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        Assert.Null(net[rowBlock * 3 + row, colBlock * 3 + col]);
                    }
                }
            }
        }

        [Fact]
        public void BuildFaceGrid_TileAtACorner_HasTheCorrectPieceId()
        {
            // (0,0,0) on a solved cube is the green/yellow/red corner.
            var grid = CubeFaceRenderer.BuildFaceGrid(Solved(), Color.Red);

            var tile = grid.Cells.Cast<Tile>().Single(t => t.Coordinate.Equals((CubeCoordinates)(0, 0, 0)));

            Assert.Equal("YGR", tile.PieceId);
        }

        [Fact]
        public void BuildNet_SharedBorders_HaveTheSamePieceIdOnBothFaces()
        {
            var net = CubeFaceRenderer.BuildNet(Solved());

            for (int row = 0; row < 3; row++)
            {
                AssertSamePieceId(net, 3 + row, 2, 3 + row, 3);
            }
        }

        private static void AssertSamePieceId(Tile?[,] net, int row1, int col1, int row2, int col2)
        {
            var tile1 = net[row1, col1];
            var tile2 = net[row2, col2];

            Assert.NotNull(tile1);
            Assert.NotNull(tile2);
            Assert.Equal(tile1!.Value.PieceId, tile2!.Value.PieceId);
        }

        [Fact]
        public void BuildNet_SharedBorders_AreTheSamePhysicalPieceOnBothFaces()
        {
            // Wherever two face-blocks touch in the printed net, the bordering cells must be the
            // exact same coordinate on both sides - i.e. the two tales of one physical piece, placed
            // next to each other - not just "some cell of the right color".
            var net = CubeFaceRenderer.BuildNet(Solved());

            for (int row = 0; row < 3; row++)
            {
                // Green (cols 0-2) | Red (cols 3-5): Green's col 2 borders Red's col 3.
                AssertSameCoordinate(net, 3 + row, 2, 3 + row, 3);

                // Red (cols 3-5) | Blue (cols 6-8): Red's col 5 borders Blue's col 6.
                AssertSameCoordinate(net, 3 + row, 5, 3 + row, 6);

                // Blue (cols 6-8) | Orange (cols 9-11): Blue's col 8 borders Orange's col 9.
                AssertSameCoordinate(net, 3 + row, 8, 3 + row, 9);
            }

            for (int col = 0; col < 3; col++)
            {
                // White (rows 0-2) | Red (rows 3-5): White's row 2 borders Red's row 3.
                AssertSameCoordinate(net, 2, 3 + col, 3, 3 + col);

                // Red (rows 3-5) | Yellow (rows 6-8): Red's row 5 borders Yellow's row 6.
                AssertSameCoordinate(net, 5, 3 + col, 6, 3 + col);
            }
        }

        private static void AssertSameCoordinate(Tile?[,] net, int row1, int col1, int row2, int col2)
        {
            var tile1 = net[row1, col1];
            var tile2 = net[row2, col2];

            Assert.NotNull(tile1);
            Assert.NotNull(tile2);
            Assert.Equal(tile1!.Value.Coordinate, tile2!.Value.Coordinate);
        }

        private static void AssertBlock(Tile?[,] net, Color expectedColor, int rowBlock, int colBlock)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    var tile = net[rowBlock * 3 + row, colBlock * 3 + col];
                    Assert.NotNull(tile);
                    Assert.Equal(expectedColor, tile!.Value.Color);
                }
            }
        }
    }
}
