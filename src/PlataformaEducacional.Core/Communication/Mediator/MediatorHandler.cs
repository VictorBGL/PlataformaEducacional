﻿using MediatR;
using PlataformaEducacional.Core.Data.EventSourcing;
using PlataformaEducacional.Core.DomainObjects;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;

namespace PlataformaEducacional.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatorHandler(IMediator mediator,
                               IEventSourcingRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
            await _eventSourcingRepository.SalvarEvento(evento);

        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediator.Publish(notificacao);
        }

        public async Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent
        {
            await _mediator.Publish(notificacao);
        }
    }
}
