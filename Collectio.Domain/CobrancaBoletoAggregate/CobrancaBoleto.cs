using System;
using Collectio.Domain.Base;
using Collectio.Domain.Base.Entities;

namespace Collectio.Domain.CobrancaBoletoAggregate
{
    public class CobrancaBoleto : BaseTenantEntity, IAggregateRoot
    {
        private DateTime _vencimento;
        private decimal _valor;

        public DateTime Vencimento => _vencimento;
        public decimal Valor => _valor;
        //public Emissor Emissor { get; set; }
    }
}
