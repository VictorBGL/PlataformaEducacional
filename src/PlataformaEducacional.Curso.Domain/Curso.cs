using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Domain
{
    public class Curso : EntityBase, IAggregateRoot
    {
        protected Curso()
        {

        }

        public Curso(string nome, bool ativo, ConteudoProgramatico conteudo)
        {
            Nome = nome;
            Ativo = ativo;
            Conteudo = conteudo;

            Validar();
        }

        public Curso(Guid id, string nome, bool ativo, ConteudoProgramatico conteudo)
        {
            Id = id;
            Nome = nome;
            Ativo = ativo;
            Conteudo = conteudo;

            Validar();
        }

        public string Nome { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public bool Ativo { get; private set; }
        public ConteudoProgramatico Conteudo { get; private set; }
        public virtual ICollection<Aula>? Aulas { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome do curso não pode estar vazio");
            Validacoes.ValidarSeNulo(Ativo, "O campo Status do curso não pode estar nulo");

            Conteudo.Validar();
        }

        public void Atualizar(Curso curso)
        {
            Nome = curso.Nome;
            Ativo = curso.Ativo;
            Conteudo = curso.Conteudo;
        }

        public void AlterarStatus(bool ativo)
        {
            Ativo = ativo;
        }

        public void AdicionarAula(Aula aula)
        {
            if (Aulas == null)
                Aulas = new List<Aula>();

            aula.Validar();
            Aulas.Add(aula);
        }

        public void AtualizarAula(Guid aulaId, Aula aula)
        {
            foreach (var aulaDb in Aulas.Where(p => p.Id == aulaId))
            {
                aula.Validar();
                aulaDb.Atualizar(aula);
            }
        }

        public void RemoverAula(Guid aulaId)
        {
            var aula = Aulas.FirstOrDefault(p => p.Id == aulaId);

            if (aula == null)
                throw new DomainException("Aula não encontrada");

            Aulas.Remove(aula);
        }
    }
}
