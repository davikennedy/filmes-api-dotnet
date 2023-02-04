using Microsoft.AspNetCore.Mvc;
using FilmesAPI.Models;

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private static List<Filme> filmes = new List<Filme>();
        private static int _id = 1;

        [HttpPost]
        public IActionResult AdicionarFilme([FromBody]Filme filme)
        {
            filme.Id = _id++;
            filmes.Add(filme);

            // Boas práticas para indicar a localização do recurso criado
            return CreatedAtAction(nameof(RecuperarFilmesPorId), new { Id = filme.Id }, filme);
        }

        [HttpGet]
        public IActionResult RecuperarFilmes()
        {
            return Ok(filmes);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperarFilmesPorId(int id)
        {
            var filme = filmes.FirstOrDefault(filme => filme.Id == id);
            
            if (filme != null)
            {
                return Ok(filme);
            }

            return NotFound();
        }
    }
}
