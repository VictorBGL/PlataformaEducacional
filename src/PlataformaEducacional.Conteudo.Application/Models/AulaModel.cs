using PlataformaEducacional.Conteudo.Domain;

namespace PlataformaEducacional.Conteudo.Application.Models
{
    public class AulaModel
    {
        public string? Titulo { get; set; }
        public string? Duracao { get; set; }
        public Guid CursoId { get; private set; }
        public bool Ativo { get; private set; }
    }
}
