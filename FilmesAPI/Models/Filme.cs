using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; internal set; }

    [Required (ErrorMessage = "O título do filme é obrigatório")]
    public string Titulo { get; set; }
    
    [Required (ErrorMessage = "O diretor do filme é obrigatório")]
    public string Diretor { get; set; }

    [Required]
    public string Genero { get; set; }

    [Required]
    [Range(30, 300, ErrorMessage = "A duração deve ter entre 30 e 300 minutos")]
    public int Duracao { get; set; }
    public virtual ICollection<Sessao> Sessoes { get; set; }
}
