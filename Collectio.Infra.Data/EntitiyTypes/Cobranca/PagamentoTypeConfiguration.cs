using Collectio.Domain.CobrancaAggregate.Pagamentos;
using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.Cobranca
{
    public class PagamentoTypeConfiguration : BaseTenantEntityTypeConfiguration<Pagamento>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Pagamento> builder)
        {
            builder.Property(p => p.Valor);
        }
    }
}