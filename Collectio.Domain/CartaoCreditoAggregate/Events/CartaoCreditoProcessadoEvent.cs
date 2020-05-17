using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class CartaoCreditoProcessadoEvent : IDomainEvent
    {
        private string _cartaoId;
        private string _numero;
        private string _token;

        public string CartaoId => _cartaoId;
        public string Numero => _numero;
        public string Token => _token;

        public CartaoCreditoProcessadoEvent(string cartaoId, string numero, string token)
        {
            _cartaoId = cartaoId;
            _numero = numero;
            _token = token;
        }
    }
}