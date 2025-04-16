using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Financeiro.Domain
{
    public class Pagamento : EntityBase, IAggregateRoot
    {
        public Pagamento(Guid alunoId, Guid matriculaId, decimal valor, DadosCartao dadosCartao)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            Valor = valor;
            Data = DateTime.UtcNow;
            DadosCartao = dadosCartao;
            Status = StatusPagamento.Pendente();
        }

        public Guid AlunoId { get; private set; }
        public Guid MatriculaId { get; private set; }

        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; }
        public DadosCartao DadosCartao { get; private set; }
        public StatusPagamento Status { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeNulo(Data, "O campo Data do pagamento não pode ser nulo");
        }

        public void ConfirmarPagamento()
        {
            Status = StatusPagamento.Confirmado();
        }

        public void RejeitarPagamento(string motivo)
        {
            Status = StatusPagamento.Rejeitado(motivo);
        }
    }
}
