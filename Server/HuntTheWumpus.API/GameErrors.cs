namespace HuntTheWumpus.API
{
	public static class GameErrors
	{
		public static readonly string GameAlreadyHasBeedStarted = "Игра уже была запущена.";
		public static string PlayerIsNotInGame(Guid playerId) =>
			$"Игрок с id {playerId} не участвуeт в игре.";
	}
}
