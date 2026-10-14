namespace RubiksCube.Solver
{
    /// <summary>
    /// A strategy that searches for a sequence of <see cref="RotationCommand" />s turning a given
    /// cube state into a solved one. Different implementations trade off optimality, speed and
    /// memory differently; see the README for the list of approaches.
    /// </summary>
    public interface ICubeSolver
    {
        CubeSolverResult Solve(Cube start);
    }
}
