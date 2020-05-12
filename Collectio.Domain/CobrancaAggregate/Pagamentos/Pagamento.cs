using System;
using Collectio.Domain.Base;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Pagamentos
{
    public class Pagamento : BaseTenantEntity
    {
        private decimal _valor;

        public decimal Valor => _valor;

        public Pagamento(decimal valor)
        {
            _valor = valor;
        }

        protected override IValidator ValidatorFactory() 
            => new PagamentoValidator();
    }
}