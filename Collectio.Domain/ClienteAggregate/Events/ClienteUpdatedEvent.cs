using Collectio.Domain.Base;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class ClienteUpdatedEvent : IDomainEvent
    {
        private Cliente _cliente;
        public Cliente cliente => _cliente;

        public ClienteUpdatedEvent(Cliente cliente) 
            => _cliente = cliente;
    }
}