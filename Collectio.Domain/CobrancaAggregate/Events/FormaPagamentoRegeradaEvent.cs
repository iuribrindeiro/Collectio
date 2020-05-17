using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class FormaPagamentoRegeradaEvent : IDomainEvent
    {
        private FormaPagamentoValueObject _formaPagamentoAnterior;
        private FormaPagamentoValueObject _formaPagamento;

        public FormaPagamentoValueObject FormaPagamentoAnterior => _formaPagamentoAnterior;
        public FormaPagamentoValueObject FormaPagamento => _formaPagamento;

        public FormaPagamentoRegeradaEvent(FormaPagamentoValueObject formaPagamentoAnterior, FormaPagamentoValueObject formaPagamento)
        {
            _formaPagamentoAnterior = formaPagamentoAnterior;
            _formaPagamento = formaPagamento;
        }
    }
}