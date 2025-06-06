namespace PlataformaEducacional.Aluno.Application.Models
{
    public class CertificadoResponseModel
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public string NomeCurso { get; set; }
        public DateTime DataEmissao { get; set; }
    }
}
