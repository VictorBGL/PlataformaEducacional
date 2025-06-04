namespace PlataformaEducacional.Financeiro.Apllication.Models
{
    public class PagamentoModel
    {
        public string NomeTitular { get; set; }
        public string NumeroCartao { get; set; }
        public DateTime Validade { get; set; }
        public decimal ValorCurso { get; set; }
        public string CvvCartao { get; private set; }
    }
}
