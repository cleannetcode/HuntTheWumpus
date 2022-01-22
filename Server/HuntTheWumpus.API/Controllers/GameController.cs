using Microsoft.AspNetCore.Mvc;

namespace HuntTheWumpus.API.Controllers
{
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

		[HttpGet]
		public IActionResult Start()
		{
			if (_game.IsRunning)
			{
				return BadRequest(GameErrors.GameAlreadyHasBeedStarted);
			}

			var playerId = Guid.NewGuid();
			var result = _game.Start(playerId);

			if (result)
			{
				return Ok();
			}
			else
			{
				return BadRequest(GameErrors.GameAlreadyHasBeedStarted);
			}
		}

		[HttpGet]
		public IActionResult Connect()
		{
		}

		[HttpGet]
		public bool Restart()
		{
		}
	}
}
