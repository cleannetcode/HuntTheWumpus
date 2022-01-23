namespace HuntTheWumpus.API
{
	public sealed class Game
	{
		private readonly List<Guid> _players;

		public Game()
		{
			_players = new List<Guid>();
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
	}
}
