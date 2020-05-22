using Collectio.Domain.Base;
using System;

namespace Collectio.Domain.CobrancaAggregate.Pagamentos
{
    public class Pagamento : BaseOwnerEntity
    {
        public Guid CobrancaId { get; private set; }
        public decimal Valor { get; private set; }

        private Pagamento() {}

        public Pagamento(decimal valor)
        {
            Valor = valor;
        }
    }
}