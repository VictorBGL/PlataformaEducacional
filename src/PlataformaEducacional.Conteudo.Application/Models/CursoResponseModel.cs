namespace PlataformaEducacional.Conteudo.Application.Models
{
    public class CursoResponseModel
    {
        public Guid? Id { get; set; }
        public string? Nome { get; set; }
        public bool Ativo { get; set; }
        public ConteudoProgramaticoModel? Conteudo { get; set; }
        public ICollection<AulaResponseModel>? Aulas { get; set; }
    }
}
