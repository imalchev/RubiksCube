using System;
using System.Linq;
using RubiksCube;

namespace RubiksCube.Cli
{
    public static class Program
    {
        private static readonly Random Random = new Random();

        public static void Main(string[] args)
        {
            AnsiConsole.EnableTrueColor();

            var session = new CubeSession();

            Console.WriteLine("Rubik's Cube CLI. Type 'help' for commands.");

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();

                if (line == null)
                {
                    break;
                }

                var trimmed = line.Trim();
                if (trimmed.Length == 0)
                {
                    continue;
                }

                var parts = trimmed.Split((char[]?)null, 2, StringSplitOptions.RemoveEmptyEntries);
                var command = parts[0].ToLowerInvariant();
                var rest = parts.Length > 1 ? parts[1] : string.Empty;

                try
                {
                    if (!Dispatch(command, rest, session))
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <returns><c>false</c> if the REPL should stop.</returns>
        private static bool Dispatch(string command, string rest, CubeSession session)
        {
            switch (command)
            {
                case "exit":
                case "quit":
                    return false;

                case "help":
                    PrintHelp();
                    return true;

                case "init":
                    RunInit(session);
                    return true;

                case "show":
                case "s":
                    RunShow(session);
                    return true;

                case "rotate":
                case "r":
                    RunRotate(session, rest);
                    return true;

                case "history":
                    RunHistory(session);
                    return true;

                default:
                    Console.WriteLine($"Unknown command '{command}'. Type 'help' for commands.");
                    return true;
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("  init             Initialize the cube (solved, scrambled, or from a move sequence).");
            Console.WriteLine("  show | s         Print the cube's state id and an opened-up box (all 6 sides, every tile's coordinate and color).");
            Console.WriteLine("  rotate | r <m..> Apply one or more moves, e.g. \"rotate Red Blue' Green2\".");
            Console.WriteLine("  history          List the moves applied since the last init.");
            Console.WriteLine("  help             Show this message.");
            Console.WriteLine("  exit | quit      Exit the CLI.");
            Console.WriteLine();
            Console.WriteLine("Move notation: a face color (Red, Orange, Yellow, White, Green, Blue), optionally");
            Console.WriteLine("followed by ' for the inverse quarter turn or 2 for a half turn. Case-insensitive.");
        }

        private static void RunInit(CubeSession session)
        {
            Console.WriteLine("How do you want to initialize the cube?");
            Console.WriteLine("  1) Solved cube");
            Console.WriteLine("  2) Scrambled with N random moves");
            Console.WriteLine("  3) Enter a specific move sequence");
            Console.Write("Choice [1-3]: ");

            var choice = Console.ReadLine()?.Trim();
            var solved = CubeBuilder.CreateOrdered().Build();

            switch (choice)
            {
                case "1":
                    session.Init(solved);
                    Console.WriteLine("Cube initialized: solved.");
                    break;

                case "2":
                    Console.Write("Number of random moves: ");
                    var countInput = Console.ReadLine();
                    if (!int.TryParse(countInput, out var count) || count < 0)
                    {
                        Console.WriteLine("That's not a valid move count. Init cancelled.");
                        return;
                    }

                    Console.Write("Random seed (blank for a random one): ");
                    var seedInput = Console.ReadLine();
                    var random = string.IsNullOrWhiteSpace(seedInput) ? Random : new Random(int.Parse(seedInput.Trim()));

                    session.Init(solved);
                    var applied = new System.Collections.Generic.List<RotationCommand>();
                    for (int i = 0; i < count; i++)
                    {
                        var move = RotationCommand.AllMoves[random.Next(RotationCommand.AllMoves.Count)];
                        session.Apply(move);
                        applied.Add(move);
                    }

                    Console.WriteLine($"Cube initialized: {count} random move(s) applied: {string.Join(' ', applied.Select(MoveNotation.ToToken))}");
                    break;

                case "3":
                    Console.Write("Enter move sequence (e.g. \"Red Blue' Green2\"): ");
                    var sequenceInput = Console.ReadLine() ?? string.Empty;
                    var moves = MoveNotation.ParseSequence(sequenceInput);

                    session.Init(solved);
                    foreach (var move in moves)
                    {
                        session.Apply(move);
                    }

                    Console.WriteLine($"Cube initialized: {moves.Count} move(s) applied.");
                    break;

                default:
                    Console.WriteLine("Not a valid choice. Init cancelled.");
                    break;
            }
        }

        private static void RunShow(CubeSession session)
        {
            if (session.Current == null)
            {
                Console.WriteLine("No cube initialized yet. Run 'init' first.");
                return;
            }

            CubeFaceRenderer.Render(session.Current);
        }

        private static void RunRotate(CubeSession session, string rest)
        {
            if (session.Current == null)
            {
                Console.WriteLine("No cube initialized yet. Run 'init' first.");
                return;
            }

            if (string.IsNullOrWhiteSpace(rest))
            {
                Console.WriteLine("Usage: rotate <move> [move...], e.g. \"rotate Red Blue' Green2\".");
                return;
            }

            var moves = MoveNotation.ParseSequence(rest);
            foreach (var move in moves)
            {
                session.Apply(move);
            }

            Console.WriteLine($"Applied: {string.Join(' ', moves.Select(MoveNotation.ToToken))}");
        }

        private static void RunHistory(CubeSession session)
        {
            if (session.History.Count == 0)
            {
                Console.WriteLine("No moves applied since init.");
                return;
            }

            Console.WriteLine(string.Join(' ', session.History.Select(MoveNotation.ToToken)));
        }
    }
}
