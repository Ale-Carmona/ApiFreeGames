using ApiFreeGames.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiFreeGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly GameService _service;
        public GamesController(GameService service)
        {
            _service = service;
        }

        // GET api/games
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var games = await _service.GetAllGames();
            return Ok(games);
        }

        // GET api/games/540
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await _service.GetGameById(id);

            if (game == null)
                return NotFound(new { message = "Juego no encontrado" });

            return Ok(game);
        }
    }
}
