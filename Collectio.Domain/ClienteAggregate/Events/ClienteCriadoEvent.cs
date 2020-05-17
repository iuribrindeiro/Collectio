using Collectio.Domain.Base;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class ClienteCriadoEvent : IDomainEvent
    {
        private readonly Cliente _cliente;
        public Cliente Cliente => _cliente;

        public ClienteCriadoEvent(Cliente cliente) 
            => _cliente = cliente;
    }
}