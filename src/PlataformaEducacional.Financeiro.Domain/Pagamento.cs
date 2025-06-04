using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Financeiro.Domain
{
    public class Pagamento : EntityBase, IAggregateRoot
    {
        public Pagamento()
        {

        }

        public Pagamento(Guid alunoId, Guid cursoId, decimal valor, DadosCartao dadosCartao)
        {
            AlunoId = alunoId;
            CursoId = cursoId;

            Valor = valor;
            Data = DateTime.Now;
            DadosCartao = dadosCartao;
            Status = StatusPagamento.Pendente();

            Validar();
        }

        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }

        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; }
        public DadosCartao DadosCartao { get; private set; }
        public StatusPagamento Status { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeNulo(Valor, "O campo Valor do pagamento não pode ser nulo");
            Validacoes.ValidarSeNulo(Data, "O campo Data do pagamento não pode ser nulo");
        }

        public bool CartaoValido(string numeroCartao)
        {
            if (DadosCartao.NumeroMascarado == "9999999999999999" && DadosCartao.CvvCartao == "123")
                return true;

            return false;
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
