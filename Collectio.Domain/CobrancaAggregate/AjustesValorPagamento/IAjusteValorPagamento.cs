using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public interface IAjusteValorPagamento
    {
        decimal Percentagem { get; }
        PeriodoAjuste Periodo { get; }
    }
}