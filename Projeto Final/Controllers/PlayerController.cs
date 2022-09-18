using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto_Final.Dto;
using Projeto_Final.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Final.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class PlayerController : ControllerBase
    {
        private readonly PlayerContext _context;

        public PlayerController(PlayerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Player>>> Get([FromQuery] int page, [FromQuery] int maxResults)
        {
            if (page < 1 || maxResults < 1)
            {
                return BadRequest("É necessário informar um valor maior que 0 (zero) em {page} e {maxResults}.");
            }

            var paginedPlayers = await _context.Players.Skip((page - 1) * page).Take(maxResults).OrderBy(x => x.Id).ToListAsync();
            return Ok(paginedPlayers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Player>> Get(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound("Player não encontrado.");
            }
            
            return Ok(player);
        }

        [HttpPost("busca")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Player>>> Post([FromBody] PlayerDto player, [FromQuery] int page, [FromQuery] int maxResults)
        {
            if (page < 1 || maxResults < 1)
            {
                return BadRequest("É necessário informar um valor maior que 0 (zero) em {page} e {maxResults}.");
            }

            var filteredPlayer = await _context.Players.Where(x => x.Nick.Equals(player.Nick) && x.FirstName.Equals(player.FirstName) && x.LastName.Equals(player.LastName) && x.Game.Equals(player.Game)).Skip((page - 1) * page).Take(maxResults).OrderBy(x => x.Nick).ToListAsync();
            if (filteredPlayer.Count < 1)
            {
                return NotFound("Nenhum player foi encontrado.");
            }

            return Ok(filteredPlayer);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Player), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Player>>> Post([FromBody] PlayerDto request)
        {
            if (request.Nick == string.Empty || request.FirstName == string.Empty || request.LastName == string.Empty || request.Game == string.Empty)
            {
                return BadRequest("É necessário preencher todos os campos.");
            }

            Player player = new() { Nick = request.Nick, FirstName = request.FirstName, LastName = request.LastName, Game = request.Game };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            var players = await _context.Players.ToListAsync();
            return Created(string.Empty, players);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Player>>> Put([FromRoute] int id, [FromBody] PlayerDto request)
        {
            var dbPlayer = await _context.Players.FindAsync(id);
            if (dbPlayer == null)
            {
                return NotFound("Player não encontrado.");
            }

            dbPlayer.Nick = request.Nick;
            dbPlayer.FirstName = request.FirstName;
            dbPlayer.LastName = request.LastName;
            dbPlayer.Game = request.Game;

            await _context.SaveChangesAsync();
            var players = await _context.Players.ToListAsync();
            return Ok(players);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Player>>> Patch([FromRoute] int id, [FromBody] PlayerPatchDto request)
        {
            var dbPlayer = await _context.Players.FindAsync(id);
            if (dbPlayer == null)
            {
                return NotFound("Player não encontrado.");
            }

            if (request.Nick != null)
            {
                dbPlayer.Nick = request.Nick;
            }

            if (request.FirstName != null)
            {
                dbPlayer.FirstName = request.FirstName;
            }

            if (request.LastName != null)
            {
                dbPlayer.LastName = request.LastName;
            }

            if (request.Game != null)
            {
                dbPlayer.Game = request.Game;
            }

            await _context.SaveChangesAsync();
            var players = await _context.Players.ToListAsync();
            return Ok(players);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Player>>> Delete([FromRoute] int id)
        {
            var dbPlayer = await _context.Players.FindAsync(id);
            if (dbPlayer == null)
            {
                return NotFound("Player não encontrado.");
            }

            _context.Players.Remove(dbPlayer);
            await _context.SaveChangesAsync();
            var players = await _context.Players.ToListAsync();
            return Ok(players);
        }
    }
}
