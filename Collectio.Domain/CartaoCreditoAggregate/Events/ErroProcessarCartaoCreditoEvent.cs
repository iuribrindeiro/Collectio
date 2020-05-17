using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class ErroProcessarCartaoCreditoEvent : IDomainEvent
    {
        private string _cartaoId;
        private string _mensagemErro;

        public string CartaoId => _cartaoId;
        public string MensagemErro => _mensagemErro;

        public ErroProcessarCartaoCreditoEvent(string cartaoId, string mensagemErro)
        {
            _cartaoId = cartaoId;
            _mensagemErro = mensagemErro;
        }
    }
}