using Microsoft.AspNetCore.Mvc;

namespace HuntTheWumpus.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PlayerController : ControllerBase
	{
		private readonly ILogger<PlayerController> _logger;

		public PlayerController(ILogger<PlayerController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public bool Move()
		{

		}


		[HttpGet]
		public bool Attack()
		{

		}
	}
}
