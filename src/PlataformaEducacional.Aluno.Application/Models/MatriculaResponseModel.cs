namespace PlataformaEducacional.Aluno.Application.Models
{
    public class MatriculaResponseModel
    {
        public Guid Id { get; set; }
        public Guid CursoId { get; set; }
        public DateTime Data { get; set; }
        public string StatusMatricula { get; set; }
        public HistoricoAprendizadoModel Historico { get; set; }
    }
}
