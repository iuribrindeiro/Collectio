using Collectio.Domain.Base;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class CartaoCreditoPadraoDefinidoEvent : IDomainEvent
    {
        private string _cartaoCreditoPadraoId;
        private string _cartaoCreditoPadraoAnteriorId;

        public string CartaoCreditoPadraoId => _cartaoCreditoPadraoId;
        public string CartaoCreditoPadraoAnteiorId => _cartaoCreditoPadraoAnteriorId;

        public CartaoCreditoPadraoDefinidoEvent(string cartaoCreditoPadraoId, string cartaoCreditoPadraoAnteriorId)
        {
            _cartaoCreditoPadraoId = cartaoCreditoPadraoId;
            _cartaoCreditoPadraoAnteriorId = cartaoCreditoPadraoAnteriorId;
        }
    }
}