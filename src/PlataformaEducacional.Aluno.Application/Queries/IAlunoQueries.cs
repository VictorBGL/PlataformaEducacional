using PlataformaEducacional.Aluno.Application.Models;

namespace PlataformaEducacional.Aluno.Application.Queries
{
    public interface IAlunoQueries
    {
        Task<List<AlunoResponseModel>> FiltrarAlunos(string? nome, bool? existeMatriculaAtiva);
        Task<AlunoResponseModel> BuscarAluno(Guid id);
        Task<List<CertificadoResponseModel>> BuscarCertificadosAluno(Guid alunoId);
    }
}
