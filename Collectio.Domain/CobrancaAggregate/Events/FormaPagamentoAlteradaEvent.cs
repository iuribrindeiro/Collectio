using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class FormaPagamentoAlteradaEvent : IDomainEvent
    {
        private Cobranca _cobranca;
        private FormaPagamentoValueObject _formaPagamentoAnterior;

        public FormaPagamentoAlteradaEvent(Cobranca cobranca, FormaPagamentoValueObject formaPagamentoAnterior)
        {
            _cobranca = cobranca;
            _formaPagamentoAnterior = formaPagamentoAnterior;
        }

        public Cobranca Cobranca => _cobranca;
        public FormaPagamentoValueObject FormaPagamentoAnterior => _formaPagamentoAnterior;
    }
}