using HuntTheWumpus.API.Domain;
using HuntTheWumpus.API.Domain.GameObjects;

namespace HuntTheWumpus.API
{
    public sealed class Game
    {
        private readonly List<Guid> _players;
        private uint _steps;
        private uint _shoots;

        public Game()
        {
            _players = new List<Guid>();
            _steps = 0;
            _shoots = 0;
        }

        public bool IsRunning { get; private set; }
        public Guid[] Players => _players.ToArray();

        public bool Start(Guid playerId)
        {
            if (IsRunning)
            {
                return false;
            }

            IsRunning = true;
            _players.Add(playerId);
            return true;
        }

        public bool Restart(Guid playerId)
        {
            _players.Clear();

            IsRunning = true;
            _players.Add(playerId);
            return true;
        }


        private Player _player;
        private Wumpus _wumpus;
        private GameMap _map;

        public void Move(Direction direction)
        {
            if (!_player.IsAlive)
            {
                return;
            }

            const x = _player.X;
            const y = _player.Y;

            MoveGameObject(_player, direction);
            this.#placeFootprint(x, y);
            _steps++;
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

        private void Start()
        {
            const uint size = 5;

            var possibleCoordinates = new (uint X, uint Y)[size * size];
            var coordinate = (X: 0u, Y: 0u);

            for (var x = 0u; x < size; x++)
            {
                for (var y = 0u; y < size; y++)
                {
                    possibleCoordinates[x + y * size] = (x, y);
                }
            }

            (coordinate, possibleCoordinates) = GetRandomCoordinate(possibleCoordinates);
            _player = new Player(coordinate.X, coordinate.Y);

            (coordinate, possibleCoordinates) = GetRandomCoordinate(possibleCoordinates);
            _wumpus = new Wumpus(coordinate.X, coordinate.Y);

            var gameObjects = new List<GameObject>();
            gameObjects.Add(_player);
            gameObjects.Add(_wumpus);

            for (let i = 0; i < 2; i++)
            {
                ({ coordinate, possibleCoordinates } = this.#getRandomCoordinate(possibleCoordinates));
			gameObjects.push(new Pit(coordinate.x, coordinate.y));
                ({ coordinate, possibleCoordinates } = this.#getRandomCoordinate(possibleCoordinates));
			gameObjects.push(new Bats(coordinate.x, coordinate.y));
            }

            this.map = new GameMap(gameObjects, size);
        }

        private ((uint X, uint Y), (uint X, uint Y)[]) GetRandomCoordinate((uint X, uint Y)[] possibleCoordinates)
        {
            var random = new Random();
            var indexOfCoordinate = random.Next(0, possibleCoordinates.Length);
            var coordinate = possibleCoordinates[indexOfCoordinate];
            var newPossibleCoordinates = possibleCoordinates.Where(x => x != coordinate).ToArray();
            return (coordinate, newPossibleCoordinates);
        }


        /**
         * @param {MoveableObject} moveableObject
         * @param {Direction} direction
         * @returns
         */
        private void MoveGameObject(moveableObject, direction)
        {
            if (!direction)
            {
                throw Error('direction cannot be null or undefined');
            }

            if (!moveableObject || !moveableObject instanceof MoveableObject) {
                throw Error('only MoveableObject can be moved by Game.moveGameObject');
            }

            if (!this.map.isValidDirection(moveableObject, direction))
            {
                return;
            }

            let room = this.map.getRoom(moveableObject.x, moveableObject.y);
            room.remove(moveableObject);

            const { x, y } = moveableObject.move(direction);

            room = this.map.getRoom(x, y);
            room.add(moveableObject);
        }

	#update() {
		if (!this.player.isAlive)
{
    const result = confirm("Ты проиграл!\nПопробуешь еще раз?");

    if (result)
    {
        this.#restart();
			}

    return;
}

if (!this.wumpus.isAlive)
{
    const result = confirm("Ты победил!\nПопробуешь еще раз?");

    if (result)
    {
        this.#restart();
			}

    return;
}

const roomWithPlayer = this.map.getRoom(this.player.x, this.player.y);

const wumpus = roomWithPlayer.getObject(x => x instanceof Wumpus);
const pit = roomWithPlayer.getObject(x => x instanceof Pit);
const bats = roomWithPlayer.getObject(x => x instanceof Bats);

if (wumpus || pit)
{
    this.player.die();
    this.#update();
		}
else if (bats)
{
    const x = Math.floor(Math.random() * this.map.size);
    const y = Math.floor(Math.random() * this.map.size);

    let room = this.map.getRoom(this.player.x, this.player.y);
    room.remove(this.player);

    this.player.x = x;
    this.player.y = y;

    room = this.map.getRoom(this.player.x, this.player.y);
    room.add(this.player);
    this.#update();
		}
else
{
    const isWumpusSleep = Math.round(Math.random());
    if (!isWumpusSleep)
    {
        this.#moveGameObject(this.wumpus, Direction.random);
				const roomWithWumpus = this.map.getRoom(this.wumpus.x, this.wumpus.y);
        const player = roomWithWumpus.getObject(x => x instanceof Player);
        if (player)
        {
            this.player.die();
            this.#update();
				}
    }
}

this.#updateGameInfo();
	}


	#updateGameInfo() {
		let gameObjects = [];

for (let x = this.player.x - 1; x <= this.player.x + 1; x++)
{
    for (let y = this.player.y - 1; y <= this.player.y + 1; y++)
    {

        if (x == this.player.x && y == this.player.y)
        {
            continue;
        }

        if (x < 0 || y < 0 || x >= this.map.size || y >= this.map.size)
        {
            continue;
        }

        const room = this.map.getRoom(x, y);
        gameObjects.push(...room.getObjects());
    }
}

const wumpusInfoElement = document.getElementById('wumpus-info');
const batsInfoElement = document.getElementById('bats-info');
const pitInfoElement = document.getElementById('pit-info');

pitInfoElement.className = "hide";
batsInfoElement.className = "hide";
wumpusInfoElement.className = "hide";

for (const gameObject of gameObjects) {
    if (gameObject instanceof Pit) {
        pitInfoElement.className = "";
    }

    if (gameObject instanceof Bats) {
        batsInfoElement.className = "";
    }

    if (gameObject instanceof Wumpus) {
        wumpusInfoElement.className = "";
    }
}
	}

	}
}
