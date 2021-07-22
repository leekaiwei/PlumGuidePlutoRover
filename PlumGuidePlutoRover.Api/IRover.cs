namespace PlumGuidePlutoRover.Api
{
    public interface IRover
    {
        int X { get; }
        int Y { get; }
        Heading Heading { get; }
        string Location { get; }

        bool Travel(TravelDirection move);

        void Turn(TurnDirection direction);
    }
}