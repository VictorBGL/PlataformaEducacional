namespace PlataformaEducacional.Aluno.Application.Queries
{
    public interface IAlunoQueries
    {
        Task<IQueryable<Domain.Aluno>> BuscarAlunos();
        Task<Domain.Aluno> BuscarAluno(Guid id);
    }
}
