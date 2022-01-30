using Microsoft.AspNetCore.Mvc;

namespace HuntTheWumpus.API.Controllers
{
    public class ConnectionRequest
    {
        public Guid PlayerId { get; set; }
    }

    public class NewGame
    {
        public GameState GameState { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly Game _game;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
            _game = GameRepository.Get();
        }

        [HttpPost("newGame")]
        public IActionResult Start()
        {
            if (_game.IsRunning)
            {
                _logger.LogWarning(GameErrors.GameAlreadyHasBeedStarted);
                return BadRequest(new ProblemDetails { Detail = GameErrors.GameAlreadyHasBeedStarted });
            }

            var playerId = Guid.NewGuid();
            const uint mapSize = 5;
            var (playerX, playerY) = _game.Start(playerId, mapSize);

            _logger.LogInformation("Игра успешно запущена");
            return Ok(new
            {
                PlayerId = playerId,
                MapSize = mapSize
            });
        }


        [HttpPost("connection")]
        public IActionResult Connect([FromBody] ConnectionRequest request)
        {
            var playerId = request.PlayerId;

            var isPlayerInGame = _game.Players.Contains(playerId);

            if (!isPlayerInGame)
            {
                _logger.LogWarning(GameErrors.PlayerIsNotInGame(playerId));
                return BadRequest(new ProblemDetails { Detail = GameErrors.PlayerIsNotInGame(playerId) });
            }

            return Ok();
        }

        [HttpPut("newGame")]
        public IActionResult Restart()
        {
            var playerId = Guid.NewGuid();
            _game.Restart(playerId);

            _logger.LogInformation("Игра успешно запущена");

            var cookieOptions = new CookieOptions
            {
                Path = "./",
                Domain = "localhost"
            };

            HttpContext.Response.Cookies.Append("PlyerId", playerId.ToString(), cookieOptions);
            return Ok(new { PlayerId = playerId });
        }
    }
}
