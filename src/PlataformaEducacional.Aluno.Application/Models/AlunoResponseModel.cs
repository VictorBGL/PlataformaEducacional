namespace PlataformaEducacional.Aluno.Application.Models
{
    public class AlunoResponseModel
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }

        public IEnumerable<MatriculaResponseModel>? Matriculas { get; set; }
    }
}
