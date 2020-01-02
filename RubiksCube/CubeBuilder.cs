namespace RubiksCube
{
    public class CubeBuilder
    {
        private readonly IPice[,,] _state;

        public static CubeBuilder Create()
        {
            return new CubeBuilder();
        }

        private CubeBuilder()
        {
            _state = new IPice[3, 3, 3];

            // initialize centers of all the walls
            _state[1, 0, 1] = new CentralPiece(Color.Yellow);   // opposite to White 
            _state[1, 2, 1] = new CentralPiece(Color.White);    // opposite to Yellow

            _state[1, 1, 0] = new CentralPiece(Color.Red);      // opposite to Orange
            _state[1, 1, 2] = new CentralPiece(Color.Orange);   // opposite to Red

            _state[0, 1, 1] = new CentralPiece(Color.Green);    // opposite to Blue
            _state[2, 1, 1] = new CentralPiece(Color.Blue);     // opposite to Green
        }

        public CubeBuilder AddPiece(TwoCornerPiece piece, int x, int y, int z)
        {
            return this;
        }
    }
}