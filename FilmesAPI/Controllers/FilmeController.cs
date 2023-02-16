﻿using Microsoft.AspNetCore.Mvc;
using FilmesAPI.Models;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um filme ao banco de dados
        /// </summary>
        /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionarFilme([FromBody]CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);
            _context.Filmes.Add(filme);
            _context.SaveChanges();

            // Boas práticas para indicar a localização do recurso criado
            return CreatedAtAction(nameof(RecuperarFilmesPorId), new { Id = filme.Id }, filme);
        }

        [HttpGet()]
        public IEnumerable<ReadFilmeDto> RecuperarFilmes(
            [FromQuery] int skip = 0, 
            [FromQuery] int take = 10,
            [FromQuery] string? nomeCinema = null)
        {
            if(nomeCinema == null) 
            {
                //filme?skip=0&take=10
                return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());
            }

            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take)
                .Where(filme => filme.Sessoes
                    .Any(sessao => sessao.Cinema.Nome == nomeCinema)).ToList());
        }

        [HttpGet("{id}")]
        public IActionResult RecuperarFilmesPorId(int id)
        {
            var filme = _context.Filmes.Find(id);            
            
            if (filme != null)
            {
                ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);
                return Ok(filmeDto);
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            var filme = _context.Filmes.Find(id);

            if (filme == null) return NotFound();

            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {
            var filme = _context.Filmes.Find(id);
            if (filme == null) return NotFound();

            var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);            
            patch.ApplyTo(filmeParaAtualizar, ModelState);

            if (!TryValidateModel(filmeParaAtualizar)) return ValidationProblem();

            _mapper.Map(filmeParaAtualizar, filme);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoverFilme(int id)
        {
            var filme = _context.Filmes.Find(id);

            if (filme == null) return NotFound();

            _context.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
