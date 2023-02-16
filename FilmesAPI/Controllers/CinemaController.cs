using Microsoft.AspNetCore.Mvc;
using FilmesAPI.Models;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace CinemasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CinemaController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public CinemaController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um Cinema ao banco de dados
        /// </summary>
        /// <param name="cinemaDto">Objeto com os campos necessários para criação de um Cinema</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionarCinema([FromBody] CreateCinemaDto cinemaDto)
        {
            Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();

            // Boas práticas para indicar a localização do recurso criado
            return CreatedAtAction(nameof(RecuperarCinemasPorId), new { Id = cinema.Id }, cinema);
        }

        /*[HttpGet()]
        public IEnumerable<Cinema> RecuperarCinemas([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            //Cinema?skip=0&take=10
            return _context.Cinemas.Skip(skip).Take(take);
        }*/

        [HttpGet()]
        public IEnumerable<ReadCinemaDto> RecuperarCinemas()
        {
            var listaDeCinemas = _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.ToList());
            return listaDeCinemas;
        }

        [HttpGet("{id}")]
        public IActionResult RecuperarCinemasPorId(int id)
        {
            var cinema = _context.Cinemas.Find(id);

            if (cinema != null)
            {
                ReadCinemaDto cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
                return Ok(cinemaDto);
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
        {
            var cinema = _context.Cinemas.Find(id);

            if (cinema == null) return NotFound();

            _mapper.Map(cinemaDto, cinema);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarCinemaParcial(int id, JsonPatchDocument<UpdateCinemaDto> patch)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null) return NotFound();

            var cinemaParaAtualizar = _mapper.Map<UpdateCinemaDto>(cinema);
            patch.ApplyTo(cinemaParaAtualizar, ModelState);

            if (!TryValidateModel(cinemaParaAtualizar)) return ValidationProblem();

            _mapper.Map(cinemaParaAtualizar, cinema);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoverCinema(int id)
        {
            var cinema = _context.Cinemas.Find(id);

            if (cinema == null) return NotFound();

            _context.Remove(cinema);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
