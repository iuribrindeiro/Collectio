using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class FormaPagamentoRegeradaEvent : IDomainEvent
    {
        private Cobranca.FormaPagamentoValueObject _formaPagamentoAnterior;
        private Cobranca.FormaPagamentoValueObject _formaPagamento;

        public Cobranca.FormaPagamentoValueObject FormaPagamentoAnterior => _formaPagamentoAnterior;
        public Cobranca.FormaPagamentoValueObject FormaPagamento => _formaPagamento;

        public FormaPagamentoRegeradaEvent(Cobranca.FormaPagamentoValueObject formaPagamentoAnterior, Cobranca.FormaPagamentoValueObject formaPagamento)
        {
            _formaPagamentoAnterior = formaPagamentoAnterior;
            _formaPagamento = formaPagamento;
        }
    }
}