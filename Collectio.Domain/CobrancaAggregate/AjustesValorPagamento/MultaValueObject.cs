using Collectio.Domain.Base;
using System;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class MultaValueObject : IAjusteValorPagamento
    {
        private decimal _porcentagem;
        private PeriodoAjuste _periodo;

        private MultaValueObject(decimal porcentagem, PeriodoAjuste periodo)
        {
            _porcentagem = porcentagem;
            _periodo = periodo;
        }

        public static MultaValueObject AoAno(decimal porcentagem)
            => new MultaValueObject(porcentagem, PeriodoAjuste.AoAno);

        public static MultaValueObject AoMes(decimal porcentagem)
            => new MultaValueObject(porcentagem, PeriodoAjuste.AoMes);

        public static MultaValueObject AoDia(decimal porcentagem)
            => new MultaValueObject(porcentagem, PeriodoAjuste.AoDia);

        public decimal Percentagem => _porcentagem;
        public PeriodoAjuste Periodo => _periodo;
    }
}