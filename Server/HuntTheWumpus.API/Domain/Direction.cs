namespace HuntTheWumpus.API.Domain
{
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Right = 3,
        Left = 4
    }

    public static class DirectionModule
    {
        public static Direction GetRandomDirection(Random random)
        {
            return (Direction)random.Next(0, 5);
        }
    }
}
