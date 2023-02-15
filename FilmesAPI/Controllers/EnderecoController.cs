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
    public class EnderecoController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public EnderecoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um Cinema ao banco de dados
        /// </summary>
        /// <param name="enderecoDto">Objeto com os campos necessários para criação de um Cinema</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionarCinema([FromBody] CreateEnderecoDto enderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();

            // Boas práticas para indicar a localização do recurso criado
            return CreatedAtAction(nameof(RecuperarEnderecosPorId), new { Id = endereco.Id }, endereco);
        }

        [HttpGet()]
        public IEnumerable<Endereco> RecuperarEnderecos([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            //Endereco?skip=0&take=10
            return _context.Enderecos.Skip(skip).Take(take);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperarEnderecosPorId(int id)
        {
            var endereco = _context.Enderecos.Find(id);

            if (endereco != null)
            {
                ReadEnderecoDto enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
                return Ok(enderecoDto);
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarCinema(int id, [FromBody] UpdateEnderecoDto enderecoDto)
        {
            var endereco = _context.Enderecos.Find(id);

            if (endereco == null) return NotFound();

            _mapper.Map(enderecoDto, endereco);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarCinemaParcial(int id, JsonPatchDocument<UpdateEnderecoDto> patch)
        {
            var endereco = _context.Enderecos.Find(id);
            if (endereco == null) return NotFound();

            var cinemaParaAtualizar = _mapper.Map<UpdateEnderecoDto>(endereco);
            patch.ApplyTo(cinemaParaAtualizar, ModelState);

            if (!TryValidateModel(cinemaParaAtualizar)) return ValidationProblem();

            _mapper.Map(cinemaParaAtualizar, endereco);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoverCinema(int id)
        {
            var endereco = _context.Enderecos.Find(id);

            if (endereco == null) return NotFound();

            _context.Remove(endereco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
