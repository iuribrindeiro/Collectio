namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class DescontoValueObject : IAjusteValorPagamento
    {
        private decimal _porcentagem;
        private PeriodoAjuste _periodo;

        private DescontoValueObject(decimal porcentagem, PeriodoAjuste periodo)
        {
            _porcentagem = porcentagem;
            _periodo = periodo;
        }

        public static DescontoValueObject AoAno(decimal porcentagem)
            => new DescontoValueObject(porcentagem, PeriodoAjuste.AoAno);

        public static DescontoValueObject AoMes(decimal porcentagem)
            => new DescontoValueObject(porcentagem, PeriodoAjuste.AoMes);

        public static DescontoValueObject AoDia(decimal porcentagem)
            => new DescontoValueObject(porcentagem, PeriodoAjuste.AoDia);

        public decimal Percentagem => _porcentagem;
        public PeriodoAjuste Periodo => _periodo;
    }
}