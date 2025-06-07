using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Domain
{
    public class Aula : EntityBase
    {
        protected Aula()
        {

        }

        public Aula(string titulo, string duracao, Guid cursoId, bool ativo)
        {
            Titulo = titulo;
            Duracao = duracao;
            CursoId = cursoId;
            DataCadastro = DateTime.Now;
            Ativo = ativo;

            Validar();
        }

        public Aula(Guid id, string titulo, string duracao, Guid cursoId, bool ativo)
        {
            Id = id;
            Titulo = titulo;
            Duracao = duracao;
            CursoId = cursoId;
            DataCadastro = DateTime.Now;
            Ativo = ativo;

            Validar();
        }

        public string Titulo { get; private set; }
        public string Duracao { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public Guid CursoId { get; private set; }
        public bool Ativo { get; private set; }
        public virtual Curso Curso { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeVazio(Titulo, "O campo Titulo da aula não pode estar vazio");
            Validacoes.ValidarSeNulo(Duracao, "O campo Duração da aula não pode ser nulo");
            Validacoes.ValidarSeDiferente(CursoId, Guid.Empty, "O campo CursoId da aula não pode ser nulo");
        }

        public void Atualizar(Aula aula)
        {
            Titulo = aula.Titulo;
            Duracao = aula.Duracao;
            Ativo = aula.Ativo;
        }
    }
}
