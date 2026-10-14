namespace RubiksCube.Solver
{
    /// <summary>
    /// The solved reference cube and the handful of checks derived from it that every solver needs:
    /// whether a cube is already solved, and (for heuristics) how far a given slot is from solved.
    /// </summary>
    public static class SolvedCube
    {
        public static Cube Instance { get; } = CubeBuilder.CreateOrdered().Build();

        private static readonly Color[] FaceCenters =
        {
            Color.White, Color.Yellow, Color.Red, Color.Orange, Color.Green, Color.Blue
        };

        /// <summary>
        /// A cube is solved when every face shows a single color across all 9 of its tales.
        /// </summary>
        public static bool IsSolved(Cube cube)
        {
            foreach (var faceCenter in FaceCenters)
            {
                var colors = cube.GetSideColors(faceCenter);
                var first = colors[0, 0];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (colors[i, j] != first)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
