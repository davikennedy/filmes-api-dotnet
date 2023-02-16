﻿using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models
{
    public class Sessao
    {
        [Key]
        [Required]
        public int Id { get; set; }

        //Relacionamento 1 : n
        [Required]
        public int FilmeId { get; set; }        
        public virtual Filme Filme { get; set; }

        [Required]
        public int? CinemaId { get; set; }
        public virtual Cinema Cinema { get; set; }
    }
}
