namespace HuntTheWumpus.API
{
	public class Game
	{
		private readonly List<Guid> _players;

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
	}
}
