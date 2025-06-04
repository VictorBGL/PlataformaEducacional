namespace PlataformaEducacional.Financeiro.Domain
{
    public class DadosCartao
    {
        public DadosCartao()
        {

        }

        public DadosCartao(string numeroCartao, string nomeTitular, string cvvCartao, DateTime validade)
        {
            NumeroMascarado = Mascarar(numeroCartao);
            NomeTitular = nomeTitular;
            CvvCartao = cvvCartao;
            Validade = validade;
        }

        public string NumeroMascarado { get; private set; }
        public string NomeTitular { get; private set; }
        public DateTime Validade { get; private set; }
        public string CvvCartao { get; private set; }


        private string Mascarar(string numero)
        {
            if (numero.Length < 4) return "****";
            return $"**** **** **** {numero[^4..]}";
        }
    }
}
