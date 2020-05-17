using Collectio.Domain.Base;
using Collectio.Domain.ClienteAggregate.CartaoCreditoModels;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class CartaoCreditoCriadoEvent : IDomainEvent
    {
        private DadosCartaoValueObject _dadosCartao;
        private string _cartaoId;

        public DadosCartaoValueObject DadosCartao => _dadosCartao;
        public string CartaoId => _cartaoId;

        public CartaoCreditoCriadoEvent(DadosCartaoValueObject dadosCartao, string cartaoId)
        {
            _dadosCartao = dadosCartao;
            _cartaoId = cartaoId;
        }
    }
}