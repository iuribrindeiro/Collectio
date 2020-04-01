using Collectio.Domain.Base;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class ClienteCreatedEvent : IDomainEvent
    {
        private readonly Cliente _cliente;
        public Cliente Cliente => _cliente;

        public ClienteCreatedEvent(Cliente cliente) 
            => _cliente = cliente;
    }
}