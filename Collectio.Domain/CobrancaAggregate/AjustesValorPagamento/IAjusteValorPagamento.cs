using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public interface IAjusteValorPagamento
    {
        decimal Valor { get; }
        PeriodoAjuste Periodo { get; }
    }
}