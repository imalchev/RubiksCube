using System;
using System.Collections.Generic;
using System.Linq;
using RubiksCube;

namespace RubiksCube.Cli
{
    /// <summary>
    /// Parses and formats moves as `&lt;ColorName&gt;[modifier]` tokens, e.g. <c>Red</c>, <c>Red'</c>,
    /// <c>Red2</c> (case-insensitive). No modifier is one quarter turn, <c>'</c> is the inverse
    /// quarter turn, <c>2</c> is a half turn. Color names are used instead of the usual
    /// Up/Down/Front/Back/Left/Right letters because this project's faces are already identified by
    /// color, and reusing a letter like "R" would be ambiguous (Right vs. Red).
    /// </summary>
    public static class MoveNotation
    {
        public static RotationCommand Parse(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new FormatException("A move token can't be empty.");
            }

            var trimmed = token.Trim();

            int turnovers;
            string colorPart;

            if (trimmed.EndsWith("'", StringComparison.Ordinal))
            {
                turnovers = 3;
                colorPart = trimmed.Substring(0, trimmed.Length - 1);
            }
            else if (trimmed.EndsWith("2", StringComparison.Ordinal))
            {
                turnovers = 2;
                colorPart = trimmed.Substring(0, trimmed.Length - 1);
            }
            else
            {
                turnovers = 1;
                colorPart = trimmed;
            }

            if (!Enum.TryParse<Color>(colorPart, ignoreCase: true, out var color) || !CubeFaces.IsFaceColor(color))
            {
                throw new FormatException(
                    $"'{token}' isn't a valid move. Expected a face color (Red, Orange, Yellow, White, Green, Blue) " +
                    "optionally followed by ' or 2, e.g. \"Red\", \"Red'\", \"Red2\".");
            }

            var (axis, segment) = CubeFaces.GetAxis(color);
            return new RotationCommand(axis, segment, turnovers);
        }

        public static IReadOnlyList<RotationCommand> ParseSequence(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Array.Empty<RotationCommand>();
            }

            return input
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
                .Select(Parse)
                .ToList();
        }

        public static string ToToken(RotationCommand move)
        {
            var color = CubeFaces.GetFaceColor(move.RotationAxis, move.Segment);

            return move.Turnovers switch
            {
                1 => color.ToString(),
                2 => color + "2",
                3 => color + "'",
                _ => throw new ArgumentOutOfRangeException(nameof(move), move.Turnovers, "Turnovers must be 1, 2 or 3."),
            };
        }
    }
}
