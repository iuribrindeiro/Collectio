using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Domain.CobrancaAggregate
{
    public class CartaoCreditoCobranca : ValueObject
    {
        public string Numero { get; private set; }
        public string Nome { get; private set; }
        public string TenantId { get; private set; }

        public CartaoCreditoCobranca(string nome, string numero, string tenantId)
        {
            Nome = nome;
            Numero = numero;
            TenantId = tenantId;
        }
    }
}