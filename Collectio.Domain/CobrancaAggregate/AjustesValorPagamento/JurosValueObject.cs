namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class JurosValueObject : IAjusteValorPagamento
    {
        private PeriodoAjuste _periodo;
        private decimal _porcentagem;

        private JurosValueObject(decimal porcentagem, PeriodoAjuste periodo)
        {
            _porcentagem = porcentagem;
            _periodo = periodo;
        }

        public static JurosValueObject AoAno(decimal porcentagem) 
            => new JurosValueObject(porcentagem, PeriodoAjuste.AoAno);

        public static JurosValueObject AoMes(decimal porcentagem)
            => new JurosValueObject(porcentagem, PeriodoAjuste.AoMes);

        public static JurosValueObject AoDia(decimal porcentagem)
            => new JurosValueObject(porcentagem, PeriodoAjuste.AoDia);

        public decimal Percentagem => _porcentagem;
        public PeriodoAjuste Periodo => _periodo;
    }
}