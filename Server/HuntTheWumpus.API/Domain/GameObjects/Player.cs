namespace HuntTheWumpus.API.Domain.GameObjects
{
    public sealed class Player : MoveableObject
    {
        public Player(uint x, uint y) : base(x, y)
        {
            IsAlive = true;
        }

        public bool IsAlive { get; private set; }

        public void Die()
        {
            IsAlive = false;
        }

        public Arrow Shoot(Direction direction)
        {
            const uint attackRange = 1;

            var (x, y) = direction switch
            {
                Direction.Up => (X, Y - attackRange),
                Direction.Down => (X, Y + attackRange),
                Direction.Left => (X - attackRange, Y),
                Direction.Right => (X + attackRange, Y),
                _ => (0u, 0u)
            };

            return new Arrow(x, y);
        }
    }
}
