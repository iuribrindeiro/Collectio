using System.Threading.Tasks;
using MediatR;

namespace Collectio.Domain.Base
{
    public class DomainEventEmitter : IDomainEventEmitter
    {
        private readonly IMediator _mediator;

        public DomainEventEmitter(IMediator mediator) 
            => _mediator = mediator;

        public virtual async Task PublishAsync(IDomainEvent domainEvent)
            => await _mediator.Publish(domainEvent);

        public virtual async Task PublishAsync(IDomainEvent[] domainEvents)
        {
            foreach (var domainEvent in domainEvents)
                await PublishAsync(domainEvent);
        }
    }
}