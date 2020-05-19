using Collectio.Domain.Base;
using System;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class CobrancaAlteradaEvent : IDomainEvent
    {
        public decimal ValorAnterior { get; }
        public DateTime VencimentoAnterior { get; }
        public string ClienteIdAnterior { get; }
        public string EmissorIdAnterior { get; }
        public string ContaBancariaIdAnterior { get; }
        public string CartaoCreditoIdAnterior { get; }
        public string CobrancaId { get; set; }

        public CobrancaAlteradaEvent(
            decimal valorAnterior, DateTime vencimentoAnterior, string clienteIdAnterior, 
            string cartaoCreditoIdAnterior, string contabancariaIdAnterior, string cobrancaId)
        {
            ValorAnterior = valorAnterior;
            VencimentoAnterior = vencimentoAnterior;
            ClienteIdAnterior = clienteIdAnterior;
            CartaoCreditoIdAnterior = cartaoCreditoIdAnterior;
            ContaBancariaIdAnterior = contabancariaIdAnterior;
            CobrancaId = cobrancaId;
        }
    }
}