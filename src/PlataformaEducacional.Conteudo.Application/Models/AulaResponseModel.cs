namespace PlataformaEducacional.Conteudo.Application.Models
{
    public class AulaResponseModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Duracao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public Guid CursoId { get; set; }
    }
}
