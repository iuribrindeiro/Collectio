using Collectio.Domain.Base;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class CartaoCreditoRemovidoEvent : IDomainEvent
    {
        private string _clienteId;
        private string _cartaoId;

        public string ClienteId => _clienteId;
        public string CartaoId => _cartaoId;

        public CartaoCreditoRemovidoEvent(string clienteId, string cartaoId)
        {
            _clienteId = clienteId;
            _cartaoId = cartaoId;
        }
    }
}