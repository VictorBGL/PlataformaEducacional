using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Aluno.Domain
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        Task<IQueryable<Aluno>> ObterTodos();
        Task<Aluno> ObterPorId(Guid id);
        void Adicionar(Aluno aluno);
        void Atualizar(Aluno aluno);
        Task<Matricula> BuscarMatricula(Guid alunoId, Guid cursoId);
        void AtualizarMatricula(Matricula matricula);
    }
}
