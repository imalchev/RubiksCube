namespace RubiksCube
{    
    public enum RotationSegment
    {
        First = 0,
        Third = 2
    }

    public class RotationCommand
    {
        public Orientation RotationAxis { get; }
        public RotationSegment Segment { get; }
        public int Turnovers { get; }

        public RotationCommand(Orientation rotationAxis, RotationSegment segment, int turnovers = 1)
        {
            RotationAxis = rotationAxis;
            Segment = segment;
            Turnovers = turnovers % 4;
        }
    }
}