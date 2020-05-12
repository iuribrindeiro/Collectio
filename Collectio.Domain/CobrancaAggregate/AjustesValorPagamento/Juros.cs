﻿using System;
using Collectio.Domain.Base;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class Juros : BaseTenantEntity, IAjusteValorPagamento
    {
        private PeriodoAjuste _periodo;
        private decimal _valor;

        public Juros(decimal valor, PeriodoAjuste periodo)
        {
            _valor = valor;
            _periodo = periodo;
        }

        public Juros(Guid id, decimal valor, PeriodoAjuste periodo) : base(id)
        {
            _valor = valor;
            _periodo = periodo;
        }

        protected override IValidator ValidatorFactory() 
            => new AjusteValorPagamentoValidator<Juros>();

        public decimal Valor => _valor;
        public PeriodoAjuste Periodo => _periodo;
    }
}