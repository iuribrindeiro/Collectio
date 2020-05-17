using System;
using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class CobrancaAlteradaEvent : IDomainEvent
    {
        private string _contabancariaIdAnterior;
        private decimal _valorAnterior;
        private DateTime _vencimentoAnterior;
        private Guid _pagadorIdAnterior;
        private string _emissorIdAnterior;
        private Cobranca _cobranca;

        public decimal ValorAnterior => _valorAnterior;
        public DateTime VencimentoAnterior => _vencimentoAnterior;
        public Guid PagadorIdAnterior => _pagadorIdAnterior;
        public string EmissorIdAnterior => _emissorIdAnterior;
        public string ContaBancariaIdAnterior => _contabancariaIdAnterior;

        public Cobranca Cobranca => _cobranca;

        public CobrancaAlteradaEvent(decimal valorAnterior, DateTime vencimentoAnterior, Guid pagadorIdAnterior, 
            string emissorIdAnterior, string contabancariaIdAnterior, Cobranca cobranca)
        {
            _valorAnterior = valorAnterior;
            _vencimentoAnterior = vencimentoAnterior;
            _pagadorIdAnterior = pagadorIdAnterior;
            _emissorIdAnterior = emissorIdAnterior;
            _contabancariaIdAnterior = contabancariaIdAnterior;
            _cobranca = cobranca;
        }
    }
}