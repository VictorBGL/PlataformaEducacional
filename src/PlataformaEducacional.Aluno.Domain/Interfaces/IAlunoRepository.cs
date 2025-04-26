using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Aluno.Domain
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        Task<IEnumerable<Aluno>> ObterTodos();
        Task<Aluno> ObterPorId(Guid id);
        void Adicionar(Aluno pedido);
        void Atualizar(Aluno pedido);
    }
}
