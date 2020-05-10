using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public abstract class AjusteValorPagamento : BaseTenantEntity
    {
        private decimal _valor;
        private PeriodoAjuste _periodo;

        public decimal Valor => _valor;
        public PeriodoAjuste Periodo => _periodo;

        public AjusteValorPagamento(decimal valor, PeriodoAjuste periodo)
        {
            _valor = valor;
            _periodo = periodo;
        }
    }
}