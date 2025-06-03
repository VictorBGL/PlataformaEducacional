namespace PlataformaEducacional.Aluno.AntiCorruption
{
    public interface IConteudoService
    {
       Task<int> BuscarQuantidadeAulasCurso(Guid cursoId);
       Task<string> BuscarNomeCurso(Guid cursoId);
    }
}
