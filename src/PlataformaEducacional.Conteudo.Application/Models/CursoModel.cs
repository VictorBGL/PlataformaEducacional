namespace PlataformaEducacional.Conteudo.Application.Models
{
    public class CursoModel
    {
        public string? Nome { get; set; }
        public bool Ativo { get; set; }

        public ConteudoProgramaticoModel? ConteudoProgramatico { get; set; }
    }
}
