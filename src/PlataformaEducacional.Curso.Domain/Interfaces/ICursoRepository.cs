using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Conteudo.Domain
{
    public interface ICursoRepository : IRepository<Curso>
    {
        Task<IQueryable<Curso>> ObterTodos();
        Task<Curso> ObterPorId(Guid id);

        void Adicionar(Curso curso);
        void Atualizar(Curso curso);
        void Remover(Curso curso);
    }
}
