namespace HuntTheWumpus.API
{
    public static class GameRepository
    {
		private static Game _game = new();
        public static Game Get()
        {
            return _game;
        }
    }
}
