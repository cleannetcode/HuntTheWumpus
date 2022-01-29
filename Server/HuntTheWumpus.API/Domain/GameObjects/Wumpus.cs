namespace HuntTheWumpus.API.Domain.GameObjects
{
    public class Wumpus : MoveableObject
    {
        public Wumpus(uint x, uint y) : base(x, y)
        {
            IsAlive = true;
        }

        public bool IsAlive { get; private set; }

        public void Die()
        {
            IsAlive = false;
        }
    }
}
