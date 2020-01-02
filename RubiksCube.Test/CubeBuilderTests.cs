using System;

using RubiksCube;

using Xunit;

namespace RubiksCube.Test
{
    public class CubeBuilderTests
    {
        [Fact]
        public void CreateDefaultCubeBuilder()
        {
            var builder = CubeBuilder.Create();

            builder.AddPiece(new TwoCornerPiece(Color.White, Color.Red), x: 1, y: 2, z: 0);
        }
    }
}
