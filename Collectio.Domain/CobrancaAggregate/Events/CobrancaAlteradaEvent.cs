using Collectio.Domain.Base;
using System;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class CobrancaAlteradaEvent : IDomainEvent
    {
        public decimal ValorAnterior { get; }
        public DateTime VencimentoAnterior { get; }
        public string ConfiguracaoEmissaoIdAnterior { get; }
        public string CobrancaId { get; set; }

        public CobrancaAlteradaEvent(
            decimal valorAnterior, DateTime vencimentoAnterior, string configuracaoEmissaoIdAnterior, string cobrancaId)
        {
            ValorAnterior = valorAnterior;
            VencimentoAnterior = vencimentoAnterior;
            ConfiguracaoEmissaoIdAnterior = configuracaoEmissaoIdAnterior;
            CobrancaId = cobrancaId;
        }
    }
}