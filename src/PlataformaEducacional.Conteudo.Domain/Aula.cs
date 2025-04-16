using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Domain
{
    public class Aula : EntityBase
    {
        public Aula(string titulo, TimeSpan duracao, Guid cursoId)
        {
            Titulo = titulo;
            Duracao = duracao;
            CursoId = cursoId;

            Validar();
        }

        public string Titulo { get; private set; }
        public TimeSpan Duracao { get; private set; }
        public Guid CursoId { get; private set; }
        public bool Ativo { get; private set; }
        public virtual Curso Curso { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeVazio(Titulo, "O campo Titulo da aula não pode estar vazio");
            Validacoes.ValidarSeNulo(Duracao, "O campo Duração da aula não pode ser nulo");
            Validacoes.ValidarSeDiferente(CursoId, Guid.Empty, "O campo CursoId da aula não pode ser nulo");
        }
    }
}
