using Collectio.Domain.Base;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class ClienteCriadoEvent : IDomainEvent
    {
        private string _clienteId;

        public string ClienteId => _clienteId;

        public ClienteCriadoEvent(string clienteId) 
            => _clienteId = clienteId;
    }
}