using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Pagamentos
{
    public class Pagamento : BaseOwnerEntity
    {
        private decimal _valor;

        public decimal Valor => _valor;

        public Pagamento(decimal valor)
        {
            _valor = valor;
        }
    }
}