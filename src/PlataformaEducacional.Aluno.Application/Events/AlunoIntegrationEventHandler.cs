using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using PlataformaEducacional.Aluno.Domain;
using PlataformaEducacional.Core.Enums;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.Aluno.Application.Events
{
    public class AlunoIntegrationEventHandler : INotificationHandler<PagamentoMatriculaEvent>
    {

        private readonly IAlunoRepository _alunoRepository;
        private readonly INotificationHandler<DomainNotification> _domainNotification;

        public AlunoIntegrationEventHandler(INotificationHandler<DomainNotification> domainNotification, IAlunoRepository alunoRepository)
        {
            _domainNotification = domainNotification;
            _alunoRepository = alunoRepository;
        }

        public async Task Handle(PagamentoMatriculaEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Status.Equals(nameof(StatusPagamentoEnum.REJEITADO)))
            {
                await _domainNotification.Handle(new DomainNotification("Pagamento", $"Falha ao realizar pagamento, dados do cartão inválidos"), new CancellationToken());
                return;
            }

            var aluno = await _alunoRepository.ObterPorId(notification.AlunoId);

            if (aluno == null)
            {
                await _domainNotification.Handle(new DomainNotification("Aluno", $"Aluno não encontrado"), new CancellationToken());
                return;
            }

            var matricula = await _alunoRepository.BuscarMatricula(aluno.Id, notification.CursoId);

            if (matricula == null)
            {
                await _domainNotification.Handle(new DomainNotification("Matricula", $"Matrícula não encontrada para este curso"), new CancellationToken());
                return;
            }

            if (matricula.Status == StatusMatriculaEnum.CONCLUIDO)
            {
                await _domainNotification.Handle(new DomainNotification("Pagamento", $"Não é possível realizar o pagamento de um curso já concluído"), new CancellationToken());
                return;
            }

            matricula.AtualizarStatus(StatusMatriculaEnum.ATIVO);

            _alunoRepository.AtualizarMatricula(matricula);
            await _alunoRepository.UnitOfWork.Commit();
        }
    }
}
