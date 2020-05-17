using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class TransacaoCartaoAprovadaEvent : IDomainEvent
    {
        private string _transacaoId;
        private string _cobrancaId;

        public string TransacaoId => _transacaoId;
        public string CobrancaId => _cobrancaId;

        public TransacaoCartaoAprovadaEvent(string transacaoId, string cobrancaId)
        {
            _transacaoId = transacaoId;
            _cobrancaId = cobrancaId;
        }
    }
}