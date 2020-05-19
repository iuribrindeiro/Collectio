using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class CartaoCreditoProcessadoEvent : IDomainEvent
    {
        private string _cartaoId;
        private string _numero;

        public string CartaoId => _cartaoId;
        public string Numero => _numero;

        public CartaoCreditoProcessadoEvent(string cartaoId, string numero)
        {
            _cartaoId = cartaoId;
            _numero = numero;
        }
    }
}