using System;
using RubiksCube;
using RubiksCube.Cli;
using Xunit;

namespace RubiksCube.Cli.Test
{
    public class MoveNotationTests
    {
        [Theory]
        [InlineData("Red", Orientation.RedOrange, RotationSegment.First, 1)]
        [InlineData("Orange", Orientation.RedOrange, RotationSegment.Third, 1)]
        [InlineData("Green", Orientation.GreenBlue, RotationSegment.First, 1)]
        [InlineData("Blue", Orientation.GreenBlue, RotationSegment.Third, 1)]
        [InlineData("Yellow", Orientation.YellowWhite, RotationSegment.First, 1)]
        [InlineData("White", Orientation.YellowWhite, RotationSegment.Third, 1)]
        public void Parse_PlainColorName_IsOneQuarterTurn(string token, Orientation axis, RotationSegment segment, int turnovers)
        {
            var move = MoveNotation.Parse(token);

            Assert.Equal(axis, move.RotationAxis);
            Assert.Equal(segment, move.Segment);
            Assert.Equal(turnovers, move.Turnovers);
        }

        [Theory]
        [InlineData("red")]
        [InlineData("RED")]
        [InlineData("ReD")]
        public void Parse_IsCaseInsensitive(string token)
        {
            var move = MoveNotation.Parse(token);

            Assert.Equal(Orientation.RedOrange, move.RotationAxis);
            Assert.Equal(RotationSegment.First, move.Segment);
            Assert.Equal(1, move.Turnovers);
        }

        [Fact]
        public void Parse_InverseModifier_IsThreeTurnovers()
        {
            var move = MoveNotation.Parse("Red'");

            Assert.Equal(3, move.Turnovers);
        }

        [Fact]
        public void Parse_HalfTurnModifier_IsTwoTurnovers()
        {
            var move = MoveNotation.Parse("Red2");

            Assert.Equal(2, move.Turnovers);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Purple")]
        [InlineData("Re")]
        [InlineData("Red3")]
        public void Parse_InvalidToken_Throws(string token)
        {
            Assert.Throws<FormatException>(() => MoveNotation.Parse(token));
        }

        [Fact]
        public void ParseSequence_MultipleTokens_ParsesEachInOrder()
        {
            var moves = MoveNotation.ParseSequence("Red Blue' Green2");

            Assert.Equal(3, moves.Count);
            Assert.Equal("Red", MoveNotation.ToToken(moves[0]));
            Assert.Equal("Blue'", MoveNotation.ToToken(moves[1]));
            Assert.Equal("Green2", MoveNotation.ToToken(moves[2]));
        }

        [Fact]
        public void ParseSequence_EmptyInput_ReturnsEmpty()
        {
            Assert.Empty(MoveNotation.ParseSequence(""));
            Assert.Empty(MoveNotation.ParseSequence("   "));
        }

        [Theory]
        [InlineData("Red")]
        [InlineData("Red'")]
        [InlineData("Red2")]
        [InlineData("White")]
        public void ToToken_RoundTripsWithParse(string token)
        {
            var move = MoveNotation.Parse(token);

            Assert.Equal(token, MoveNotation.ToToken(move));
        }
    }
}
