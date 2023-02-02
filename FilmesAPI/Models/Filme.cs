using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models
{
    public class Filme
    {
        [Required (ErrorMessage = "Insira o título do filme")]
        public string Titulo { get; set; }
        
        [Required (ErrorMessage = "Insira o nome do diretor")]
        public string Diretor { get; set; }

        [Required]
        public string Genero { get; set; }

        [Range(30, 300, ErrorMessage = "A duração deve ter entre 30 e 300 minutos")]
        public int Duracao { get; set; }
    }
}
