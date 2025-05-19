using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Conteudo.Domain
{
    public class Curso : EntityBase, IAggregateRoot
    {
        protected Curso()
        {

        }

        public Curso(string nome, ConteudoProgramatico conteudo)
        {
            Nome = nome;
            Conteudo = conteudo;

            Validar();
        }

        public string Nome { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public bool Ativo { get; private set; }
        public ConteudoProgramatico Conteudo { get; private set; }
        public virtual ICollection<Aula> Aulas { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome do curso não pode estar vazio");
        }

        public void AdicionarAula(Aula aula)
        {
            Aulas.Add(aula);
        }
    }
}
