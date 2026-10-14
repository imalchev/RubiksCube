using System;
using RubiksCube;

namespace RubiksCube.Cli
{
    /// <summary>
    /// One physical tale: the coordinate it currently sits at and the color currently showing there.
    /// </summary>
    public readonly struct Tile
    {
        public CubeCoordinates Coordinate { get; }
        public Color Color { get; }

        public Tile(CubeCoordinates coordinate, Color color)
        {
            Coordinate = coordinate;
            Color = color;
        }
    }

    /// <summary>
    /// The 3x3 tiles making up one face, indexed [row, col].
    /// </summary>
    public sealed class FaceGrid
    {
        public Color CenterColor { get; }
        public Tile[,] Cells { get; }

        public FaceGrid(Color centerColor, Tile[,] cells)
        {
            CenterColor = centerColor;
            Cells = cells;
        }
    }

    /// <summary>
    /// Builds and prints the cube as one big matrix: an opened-up box with all 6 sides visible at
    /// once, laid out as the standard unfolded cross (up/down above and below the front face, with
    /// left/front/right/back side by side):
    /// <code>
    ///             [ White  ]
    /// [ Green ] [ Red ] [ Blue ] [ Orange ]
    ///             [ Yellow ]
    /// </code>
    /// White/Yellow are the "up"/"down" faces (Y=2 is up, Y=0 is down on this cube's coordinates),
    /// Red/Orange are "front"/"back", Green/Blue are "left"/"right" — an arbitrary but fixed
    /// convention. Every one of the 54 cells is one tale, printed as its coordinate plus its color's
    /// first letter (W/Y/R/O/G/B, all six unique), with a matching console background color.
    ///
    /// Each face's own row/column directions are chosen (not just "whichever two axes vary") so
    /// that wherever two faces touch in this net, their bordering cells are the *same* coordinate —
    /// i.e. the two tales of one physical piece. E.g. Green's rightmost column and Red's leftmost
    /// column are both the (0,y,0) edge, cell for cell; White's bottom row and Red's top row are both
    /// the (x,2,0) edge.
    /// </summary>
    public static class CubeFaceRenderer
    {
        private const int CellWidth = 9;

        private static readonly (Color Face, int RowBlock, int ColBlock)[] Layout =
        {
            (Color.White, 0, 1),
            (Color.Green, 1, 0),
            (Color.Red, 1, 1),
            (Color.Blue, 1, 2),
            (Color.Orange, 1, 3),
            (Color.Yellow, 2, 1),
        };

        /// <summary>
        /// The coordinate shown at [row, col] of a given face, chosen so adjacent faces' shared
        /// borders in <see cref="Layout" /> line up cell for cell (see type-level remarks).
        /// </summary>
        private static CubeCoordinates CoordinateAt(Color face, int row, int col) => face switch
        {
            Color.Green => (0, 2 - row, 2 - col),
            Color.Red => (col, 2 - row, 0),
            Color.Blue => (2, 2 - row, col),
            Color.Orange => (2 - col, 2 - row, 2),
            Color.White => (col, 2, 2 - row),
            Color.Yellow => (col, 0, row),
            _ => throw new ArgumentOutOfRangeException(nameof(face), face, "Not a face center color."),
        };

        public static FaceGrid BuildFaceGrid(Cube cube, Color faceCenter)
        {
            var (axis, _) = CubeFaces.GetAxis(faceCenter);

            var cells = new Tile[3, 3];

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    var coordinate = CoordinateAt(faceCenter, row, col);
                    var color = cube.GetCubePiece(coordinate).GetColor(axis);
                    cells[row, col] = new Tile(coordinate, color);
                }
            }

            return new FaceGrid(faceCenter, cells);
        }

        /// <summary>
        /// Assembles all 6 faces into the single 9x12 opened-box matrix. Cells outside the cross
        /// (its 3 blank corners) are <c>null</c>.
        /// </summary>
        public static Tile?[,] BuildNet(Cube cube)
        {
            var net = new Tile?[9, 12];

            foreach (var (face, rowBlock, colBlock) in Layout)
            {
                var grid = BuildFaceGrid(cube, face);

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        net[rowBlock * 3 + row, colBlock * 3 + col] = grid.Cells[row, col];
                    }
                }
            }

            return net;
        }

        public static void Render(Cube cube)
        {
            var net = BuildNet(cube);

            var originalBackground = Console.BackgroundColor;
            var originalForeground = Console.ForegroundColor;

            for (int row = 0; row < net.GetLength(0); row++)
            {
                for (int col = 0; col < net.GetLength(1); col++)
                {
                    var tile = net[row, col];

                    if (tile == null)
                    {
                        Console.Write(new string(' ', CellWidth));
                    }
                    else
                    {
                        var (coordinate, color) = (tile.Value.Coordinate, tile.Value.Color);

                        Console.BackgroundColor = ToConsoleColor(color);
                        Console.ForegroundColor = ForegroundFor(color);
                        Console.Write($"({coordinate.X},{coordinate.Y},{coordinate.Z}){Abbreviate(color)}".PadRight(CellWidth));
                        Console.BackgroundColor = originalBackground;
                        Console.ForegroundColor = originalForeground;
                    }
                }

                Console.WriteLine();
            }
        }

        private static char Abbreviate(Color color) => color switch
        {
            Color.White => 'W',
            Color.Yellow => 'Y',
            Color.Red => 'R',
            Color.Orange => 'O',
            Color.Green => 'G',
            Color.Blue => 'B',
            _ => '?',
        };

        private static ConsoleColor ToConsoleColor(Color color) => color switch
        {
            Color.White => ConsoleColor.White,
            Color.Yellow => ConsoleColor.Yellow,
            Color.Red => ConsoleColor.Red,
            Color.Orange => ConsoleColor.DarkYellow,
            Color.Green => ConsoleColor.Green,
            Color.Blue => ConsoleColor.Blue,
            _ => ConsoleColor.Gray,
        };

        private static ConsoleColor ForegroundFor(Color color) =>
            color == Color.Blue || color == Color.Red ? ConsoleColor.White : ConsoleColor.Black;
    }
}
