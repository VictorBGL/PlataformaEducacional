using PlataformaEducacional.Conteudo.Application.Models;

namespace PlataformaEducacional.Conteudo.Application.Interfaces
{
    public interface IConteudoAppService
    {
        Task<List<CursoResponseModel>> FiltrarCursos(bool? ativo, string? nome);
        Task<CursoResponseModel> BuscarCurso(Guid id);
        Task AdicionarCurso(CursoModel model);
        Task AlterarStatusCurso(Guid id, bool ativo);
        Task AtualizarCurso(Guid id, CursoModel model);
        Task<CursoResponseModel> AdicionarAula(Guid cursoId, AulaModel model);
        Task<CursoResponseModel> AtualizarAula(Guid cursoId, Guid aulaId, AulaModel model);
        Task<CursoResponseModel> RemoverAula(Guid cursoId, Guid aulaId);
    }
}
