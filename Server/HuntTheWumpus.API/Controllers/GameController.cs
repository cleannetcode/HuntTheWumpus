using Microsoft.AspNetCore.Mvc;

namespace HuntTheWumpus.API.Controllers
{
	public class ConnectionRequest
	{
		public Guid PlayerId { get; set; }
	}

	[ApiController]
	[Route("[controller]")]
	public class GameController : ControllerBase
	{
		private readonly ILogger<GameController> _logger;
		private static Game _game = new();

		public GameController(ILogger<GameController> logger)
		{
			_logger = logger;
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
			var isSuccess = _game.Start(playerId);

			if (isSuccess)
			{
				_logger.LogInformation("Игра успешно запущена");
				return Ok(new { PlayerId = playerId });
			}

			_logger.LogWarning(GameErrors.GameAlreadyHasBeedStarted);
			return BadRequest(new ProblemDetails { Detail = GameErrors.GameAlreadyHasBeedStarted });
		}


		[HttpPost("connection")]
		public IActionResult Connect([FromBody]ConnectionRequest request)
		{
			var playerId = request.PlayerId;

			var isPlayerInGame = _game.Players.Contains(playerId);


			if(!isPlayerInGame)
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
