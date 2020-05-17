using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class TransacaoCobrancaReprocessadaEvent : IDomainEvent
    {
        private string _cobrancaId;

        public string CobrancaId => _cobrancaId;

        public TransacaoCobrancaReprocessadaEvent(string cobrancaId) 
            => _cobrancaId = cobrancaId;
    }
}