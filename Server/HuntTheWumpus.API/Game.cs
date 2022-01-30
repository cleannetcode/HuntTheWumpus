using System.Collections.Generic;
using HuntTheWumpus.API.Domain;
using HuntTheWumpus.API.Domain.GameObjects;

namespace HuntTheWumpus.API
{
    public sealed class Game
    {
        private readonly List<Guid> _players;
        private uint _steps;
        private uint _shoots;
        private Player _player;
        private Wumpus _wumpus;
        private GameMap _map;

        public Game()
        {
            _players = new List<Guid>();
            _steps = 0;
            _shoots = 0;
        }

        public bool IsRunning { get; private set; }
        public Guid[] Players => _players.ToArray();

        public (uint X, uint Y) Start(Guid playerId, uint mapSize)
        {
            if (IsRunning)
            {
                return (_player.X, _player.Y);
            }

            IsRunning = true;
            _players.Add(playerId);
            Start(mapSize);
            return (_player.X, _player.Y);
        }

        public bool Restart(Guid playerId)
        {
            _players.Clear();

            IsRunning = true;
            _players.Add(playerId);
            return true;
        }

        public (uint X, uint Y) Move(Direction direction)
        {
            if (!_player.IsAlive)
            {
                return (_player.X, _player.Y);
            }

            var x = _player.X;
            var y = _player.Y;

            MoveGameObject(_player, direction);
            PlaceFootprint(x, y);
            _steps++;

            return (_player.X, _player.Y);
        }

        public void Attack(Direction direction)
        {
            if (!_player.IsAlive)
            {
                return;
            }

            var arrow = _player.Shoot(direction);

            if (arrow.X == _wumpus.X && arrow.Y == _wumpus.Y)
            {
                _wumpus.Die();
            }

            _shoots++;
        }

        private void PlaceFootprint(uint x, uint y)
        {
            var room = _map.GetRoom(x, y);
            var oldFootprint = room.GetObject(x => x is Footprint);
            if (oldFootprint == null)
            {
                room.Add(new Footprint(x, y));
            }
        }

        private void Start(uint mapSize)
        {
            var possibleCoordinates = new (uint X, uint Y)[mapSize * mapSize];
            var coordinate = (X: 0u, Y: 0u);

            for (var x = 0u; x < mapSize; x++)
            {
                for (var y = 0u; y < mapSize; y++)
                {
                    possibleCoordinates[x + y * mapSize] = (x, y);
                }
            }

            (coordinate, possibleCoordinates) = GetRandomCoordinate(possibleCoordinates);
            _player = new Player(coordinate.X, coordinate.Y);

            (coordinate, possibleCoordinates) = GetRandomCoordinate(possibleCoordinates);
            _wumpus = new Wumpus(coordinate.X, coordinate.Y);

            var gameObjects = new List<GameObject>();
            gameObjects.Add(_player);
            gameObjects.Add(_wumpus);

            for (var i = 0; i < 2; i++)
            {
                (coordinate, possibleCoordinates) = GetRandomCoordinate(possibleCoordinates);
                gameObjects.Add(new Pit(coordinate.X, coordinate.Y));
                (coordinate, possibleCoordinates) = GetRandomCoordinate(possibleCoordinates);
                gameObjects.Add(new Bats(coordinate.X, coordinate.Y));
            }

            _map = new GameMap(gameObjects.ToArray(), mapSize);
        }

        private ((uint X, uint Y), (uint X, uint Y)[]) GetRandomCoordinate((uint X, uint Y)[] possibleCoordinates)
        {
            var random = new Random();
            var indexOfCoordinate = random.Next(0, possibleCoordinates.Length);
            var coordinate = possibleCoordinates[indexOfCoordinate];
            var newPossibleCoordinates = possibleCoordinates.Where(x => x != coordinate).ToArray();
            return (coordinate, newPossibleCoordinates);
        }

        private void MoveGameObject(MoveableObject moveableObject, Direction direction)
        {
            if (!_map.IsValidDirection(moveableObject, direction))
            {
                return;
            }

            var room = _map.GetRoom(moveableObject.X, moveableObject.Y);
            room.Remove(moveableObject);

            var (x, y) = moveableObject.Move(direction);

            room = _map.GetRoom(x, y);
            room.Add(moveableObject);
        }

        private void Update()
        {
            var random = new Random();

            if (!_player.IsAlive || !_wumpus.IsAlive)
            {
                return;
            }

            var roomWithPlayer = _map.GetRoom(_player.X, _player.Y);

            var wumpus = roomWithPlayer.GetObject(x => x is Wumpus);
            var pit = roomWithPlayer.GetObject(x => x is Pit);
            var bats = roomWithPlayer.GetObject(x => x is Bats);

            if (wumpus != null || pit != null)
            {
                _player.Die();
            }
            else if (bats != null)
            {
                var x = (uint)random.Next(0, (int)_map.Size);
                var y = (uint)random.Next(0, (int)_map.Size);

                var room = _map.GetRoom(_player.X, _player.Y);
                room.Remove(_player);

                _player.X = x;
                _player.Y = y;

                room = _map.GetRoom(_player.X, _player.Y);
                room.Add(_player);
                Update();
            }
            else
            {
                var isWumpusSleep = random.Next(0, 2) == 0;
                if (!isWumpusSleep)
                {
                    var direction = DirectionModule.GetRandomDirection(random);
                    MoveGameObject(_wumpus, direction);
                    var roomWithWumpus = _map.GetRoom(_wumpus.X, _wumpus.Y);
                    var player = roomWithWumpus.GetObject(x => x is Player);
                    if (player != null)
                    {
                        _player.Die();
                        Update();
                    }
                }
            }

            UpdateGameInfo();
        }

        private string[] UpdateGameInfo()
        {
            var gameObjects = new List<GameObject>();

            for (var x = _player.X - 1; x <= _player.X + 1; x++)
            {
                for (var y = _player.Y - 1; y <= _player.Y + 1; y++)
                {

                    if (x == _player.X && y == _player.Y)
                    {
                        continue;
                    }

                    if (x < 0 || y < 0 || x >= _map.Size || y >= _map.Size)
                    {
                        continue;
                    }

                    var room = _map.GetRoom(x, y);
                    gameObjects.AddRange(room.GetObjects());
                }
            }

            var info = new HashSet<string>();

            foreach (var gameObject in gameObjects)
            {
                if (gameObject is Pit)
                {
                    info.Add("Вы чувствуете вонь");
                }

                if (gameObject is Bats)
                {
                    info.Add("Вы слышите шелест");
                }

                if (gameObject is Wumpus)
                {
                    info.Add("Вы чувствуете скозняк");
                }
            }

            return info.ToArray();
        }
    }
}
