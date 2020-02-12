using System.Collections.Generic;

namespace RubiksCube
{
    public class Cube
    {
        private CentralPiece Yellow { get; } = new CentralPiece(Color.Yellow);
        private CentralPiece Red { get; } = new CentralPiece(Color.Red);
        private CentralPiece Blue { get; } = new CentralPiece(Color.Blue);
        private CentralPiece Green { get; } = new CentralPiece(Color.Green);
        private CentralPiece White { get; } = new CentralPiece(Color.White);
        private CentralPiece Orange { get; } = new CentralPiece(Color.Orange);

        private TwoCornerPiece YellowRed { get; } = new TwoCornerPiece(Color.Yellow, Color.Red);
        private TwoCornerPiece YellowGreen { get; } = new TwoCornerPiece(Color.Yellow, Color.Green);
        private TwoCornerPiece YellowBlue { get; } = new TwoCornerPiece(Color.Yellow, Color.Blue);
        private TwoCornerPiece YellowOrange { get; } = new TwoCornerPiece(Color.Yellow, Color.Orange);
        private TwoCornerPiece RedGreen { get; } = new TwoCornerPiece(Color.Red, Color.Green);
        private TwoCornerPiece RedBlue { get; } = new TwoCornerPiece(Color.Red, Color.Blue);
        private TwoCornerPiece RedWhite { get; } = new TwoCornerPiece(Color.Red, Color.White);
        private TwoCornerPiece BlueWhite { get; } = new TwoCornerPiece(Color.Blue, Color.White);
        private TwoCornerPiece BlueOrange { get; } = new TwoCornerPiece(Color.Blue, Color.Orange);
        private TwoCornerPiece GreenOrange { get; } = new TwoCornerPiece(Color.Green, Color.Orange);
        private TwoCornerPiece GreenWhite { get; } = new TwoCornerPiece(Color.Green, Color.White);
        private TwoCornerPiece OrangeWhite { get; } = new TwoCornerPiece(Color.Orange, Color.White);

        private ThreeCornerPiece YellowRedBlue { get; } = new ThreeCornerPiece(Color.Yellow, Color.Red, Color.Blue);
        private ThreeCornerPiece YellowRedGreen { get; } = new ThreeCornerPiece(Color.Yellow, Color.Red, Color.Green);
        private ThreeCornerPiece YellowGreenOrange { get; } = new ThreeCornerPiece(Color.Yellow, Color.Green, Color.Orange);
        private ThreeCornerPiece YellowBlueOrange { get; } = new ThreeCornerPiece(Color.Yellow, Color.Blue, Color.Orange);
        private ThreeCornerPiece RedGreenWhite { get; } = new ThreeCornerPiece(Color.Red, Color.Green, Color.White);
        private ThreeCornerPiece RedBlueWhite { get; } = new ThreeCornerPiece(Color.Red, Color.Blue, Color.White);
        private ThreeCornerPiece BlueOrangeWhite { get; } = new ThreeCornerPiece(Color.Blue, Color.Orange, Color.White);
        private ThreeCornerPiece GreenOrangeWhite { get; } = new ThreeCornerPiece(Color.Green, Color.Orange, Color.White);

        private IPiece[,,] _state = new IPiece[3, 3, 3];

        internal Cube(IPiece[,,] state)
        {
            for (int indexX = 0; indexX < 3; indexX++)            
            {
                for (int indexY = 0; indexY < 3; indexY++)
                {
                    for (int indexZ = 0; indexZ < 3; indexZ++)
                    {
                        _state[indexX, indexY, indexZ] = state[indexX, indexY, indexZ];
                    }
                }
            }
        }
    }
}
