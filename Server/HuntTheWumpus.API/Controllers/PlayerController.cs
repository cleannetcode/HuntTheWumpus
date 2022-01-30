using HuntTheWumpus.API.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HuntTheWumpus.API.Controllers
{
    public class PlayerState
    {
        public bool IsAlive { get; set; }
        public uint X { get; set; }
        public uint Y { get; set; }
    }

    public class WumpusState
    {
        public bool IsAlive { get; set; }
        public uint X { get; set; }
        public uint Y { get; set; }
    }

    public class GameState
    {
        public PlayerState Player { get; set; }
        public WumpusState Wumpus { get; set; }

        //public Stats Stats { get; set; }
        //public NearbyObjects NearbyObjects { get; set; }
        public Guid PlayerId { get; set; }
    }

    public class MoveRequest
    {
        public Direction Direction { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly Game _game;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _game = GameRepository.Get();
            _logger = logger;
        }

        [HttpPost("moveState")]
        public IActionResult Move(MoveRequest request)
        {
            var coordinate = _game.Move(request.Direction);

            return Ok(new GameState
            {
                Player = new PlayerState
                {
                    X = coordinate.X,
                    Y = coordinate.Y,
                }
            });
        }


        [HttpPost("attackState")]
        public IActionResult Attack()
        {
            return Ok();
        }
    }
}
