namespace HuntTheWumpus.API.Domain.GameObjects
{
    public abstract class MoveableObject : GameObject
    {
        protected MoveableObject(uint x, uint y) : base(x, y)
        {
        }

        public (uint X, uint Y) Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    Y--;
                    break;

                case Direction.Down:
                    Y++;
                    break;

                case Direction.Left:
                    X--;
                    break;

                case Direction.Right:
                    X++;
                    break;

                default:
                    break;
            }

            return (X, Y);
        }
    }
}
