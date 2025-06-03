namespace PlataformaEducacional.Financeiro.Domain
{
    public class DadosCartao
    {
        public DadosCartao()
        {

        }

        public DadosCartao(string numeroCartao, string nomeTitular, string validade)
        {
            NumeroMascarado = Mascarar(numeroCartao);
            NomeTitular = nomeTitular;
            Validade = validade;
        }

        public string NumeroMascarado { get; private set; }
        public string NomeTitular { get; private set; }
        public string Validade { get; private set; }


        private string Mascarar(string numero)
        {
            if (numero.Length < 4) return "****";
            return $"**** **** **** {numero[^4..]}";
        }
    }
}
