namespace PlataformaEducacional.Conteudo.Domain.Interfaces
{
    public interface ICursoService
    {
        Task<List<Curso>> FiltrarCursos(bool? ativo, string? nome);
        Task<Curso> BuscarCurso(Guid id);
        Task AdicionarCurso(Curso curso);
        Task AlterarStatusCurso(Guid id, bool ativo);
        Task AtualizarCurso(Guid id, Curso curso);
        Task<bool> AdicionarAula(Guid cursoId, Aula aula);
        Task<bool> AlterarAula(Guid cursoId, Guid aulaId, Aula aula);
        Task<bool> RemoverAula(Guid cursoId, Guid aulaId);
    }
}
