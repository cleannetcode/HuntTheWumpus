namespace HuntTheWumpus.API.Domain.GameObjects
{
    public class GameMap
    {
        private readonly Room[,] _rooms;

        public GameMap(GameObject[] gameObjects, uint size)
        {
            if (gameObjects.Length == 0)
            {
                throw new ArgumentException("argument gameObjects cannot be null or undefiend!");
            }

            Size = size;

            _rooms = new Room[size, size];

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    _rooms[x, y] = new Room();
                }
            }

            foreach (var gameObject in gameObjects)
            {
                var room = _rooms[gameObject.X, gameObject.Y];
                room.Add(gameObject);
            }
        }
        public uint Size { get; private set; }

        public Room GetRoom(uint x, uint y) => _rooms[x, y];

        public bool IsValidDirection(GameObject gameObject, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return gameObject.Y > 0;

                case Direction.Down:
                    return gameObject.Y < (Size - 1);

                case Direction.Left:
                    return gameObject.X > 0;

                case Direction.Right:
                    return gameObject.X < (Size - 1);

                default:
                    return false;
            }
        }
    }
}
