using MediatR;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.Financeiro.Domain;

namespace PlataformaEducacional.Financeiro.Apllication.Commands
{
    public class FinanceiroCommandHandler : IRequestHandler<PagamentoMatriculaCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IFinanceiroRepository _financeiroRepository;

        public FinanceiroCommandHandler(IMediatorHandler mediatorHandler, IFinanceiroRepository financeiroRepository)
        {
            _mediatorHandler = mediatorHandler;
            _financeiroRepository = financeiroRepository;
        }

        public async Task<bool> Handle(PagamentoMatriculaCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request))
                return false;

            var cartao = new DadosCartao(request.NomeTitular, request.NumeroCartao, request.CvvCartao, request.Validade);

            var pagamento = new Pagamento(request.AlunoId, request.CursoId, request.ValorCurso, cartao);

            if (pagamento.CartaoValido(request.NumeroCartao))
                pagamento.ConfirmarPagamento();
            else
                pagamento.RejeitarPagamento("Cartão inválido");

            _financeiroRepository.Adicionar(pagamento);
            await _financeiroRepository.UnitOfWork.Commit();

            await _mediatorHandler.PublicarEvento(new PagamentoMatriculaEvent(request.CursoId, request.AlunoId, pagamento.Status.Status));

            return true;
        }


        private bool ValidarComando(Command message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
