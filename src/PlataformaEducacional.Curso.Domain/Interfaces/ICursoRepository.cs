using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Conteudo.Domain
{
    public interface ICursoRepository : IRepository<Curso>
    {
        Task<IEnumerable<Curso>> ObterTodos();
        Task<Curso> ObterPorId(Guid id);

        void Adicionar(Curso curso);
        void Atualizar(Curso curso);
    }
}
