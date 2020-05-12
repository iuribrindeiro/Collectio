using System;
using Collectio.Domain.Base;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class Multa : BaseTenantEntity, IAjusteValorPagamento
    {
        private decimal _valor;
        private PeriodoAjuste _periodo;

        public Multa(decimal valor, PeriodoAjuste periodo)
        {
            _valor = valor;
            _periodo = periodo;
        }

        public Multa(Guid id, decimal valor, PeriodoAjuste periodo) : base(id)
        {
            _valor = valor;
            _periodo = periodo;
        }

        protected override IValidator ValidatorFactory()
            => new AjusteValorPagamentoValidator<Multa>();

        public decimal Valor => _valor;
        public PeriodoAjuste Periodo => _periodo;
    }
}