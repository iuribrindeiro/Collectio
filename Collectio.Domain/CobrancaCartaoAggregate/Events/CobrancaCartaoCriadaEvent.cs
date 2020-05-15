using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaCartaoAggregate.Events
{
    public class CobrancaCartaoCriadaEvent : IDomainEvent
    {
        private CobrancaCartao _cobrancaCartao;
        public CobrancaCartao CobrancaCartao => _cobrancaCartao;

        public CobrancaCartaoCriadaEvent(CobrancaCartao cobrancaCartao) 
            => _cobrancaCartao = cobrancaCartao;
    }
}