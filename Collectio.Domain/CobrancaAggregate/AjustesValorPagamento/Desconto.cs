using System;
using Collectio.Domain.Base;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class Desconto : BaseTenantEntity, IAjusteValorPagamento
    {
        private decimal _valor;
        private PeriodoAjuste _periodo;

        public Desconto(decimal valor, PeriodoAjuste periodo)
        {
            _valor = valor;
            _periodo = periodo;
        }

        public Desconto(Guid id, decimal valor, PeriodoAjuste periodo) : base(id)
        {
            _valor = valor;
            _periodo = periodo;
        }

        protected override IValidator ValidatorFactory() 
            => new AjusteValorPagamentoValidator<Desconto>();

        public decimal Valor => _valor;
        public PeriodoAjuste Periodo => _periodo;
    }
}