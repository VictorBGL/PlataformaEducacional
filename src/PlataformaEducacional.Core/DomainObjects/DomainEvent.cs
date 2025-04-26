using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.Core.DomainObjects
{
    public class DomainEvent : Event
    {
        public DomainEvent(Guid aggregateId) 
        {
            AggregateId = aggregateId;
        }
    }
}
