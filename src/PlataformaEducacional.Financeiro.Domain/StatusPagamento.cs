namespace PlataformaEducacional.Financeiro.Domain
{
    public class StatusPagamento
    {
        public StatusPagamento() { }

        private StatusPagamento(string status, string? motivo = null)
        {
            Status = status;
            MotivoRejeicao = motivo;
        }

        public string Status { get; private set; }
        public string? MotivoRejeicao { get; private set; }


        public static StatusPagamento Pendente() => new("PENDENTE");
        public static StatusPagamento Confirmado() => new("CONFIRMADO");
        public static StatusPagamento Rejeitado(string motivo) => new("REJEITADO", motivo);
    }
}
