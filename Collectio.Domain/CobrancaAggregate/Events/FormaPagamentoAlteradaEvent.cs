using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class FormaPagamentoAlteradaEvent : IDomainEvent
    {
        private string _cobrancaId;
        private FormaPagamento _formaPagamentoAnterior;

        public string CobrancaId => _cobrancaId;
        public FormaPagamento FormaPagamentoAnterior => _formaPagamentoAnterior;

        public FormaPagamentoAlteradaEvent(string cobrancaId, FormaPagamento formaPagamentoAnterio)
        {
            _cobrancaId = cobrancaId;
            _formaPagamentoAnterior = formaPagamentoAnterio;
        }
    }
}