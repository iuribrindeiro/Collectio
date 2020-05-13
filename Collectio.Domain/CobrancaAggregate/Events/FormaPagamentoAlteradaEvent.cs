using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class FormaPagamentoAlteradaEvent : IDomainEvent
    {
        private Cobranca _cobranca;
        private Cobranca.FormaPagamentoValueObject _formaPagamentoAnterior;

        public FormaPagamentoAlteradaEvent(Cobranca cobranca, Cobranca.FormaPagamentoValueObject formaPagamentoAnterior)
        {
            _cobranca = cobranca;
            _formaPagamentoAnterior = formaPagamentoAnterior;
        }

        public Cobranca Cobranca => _cobranca;
        public Cobranca.FormaPagamentoValueObject FormaPagamentoAnterior => _formaPagamentoAnterior;
    }
}