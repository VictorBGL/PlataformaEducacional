using PlataformaEducacional.Core.Enums;

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


        public static StatusPagamento Pendente() => new(nameof(StatusPagamentoEnum.PENDENTE));
        public static StatusPagamento Confirmado() => new(nameof(StatusPagamentoEnum.CONFIRMADO));
        public static StatusPagamento Rejeitado(string motivo) => new(nameof(StatusPagamentoEnum.REJEITADO), motivo);
    }
}
