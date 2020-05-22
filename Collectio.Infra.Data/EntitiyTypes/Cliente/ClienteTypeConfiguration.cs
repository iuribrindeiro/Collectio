using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.Cliente
{
    public class ClienteTypeConfiguration : BaseTenantEntityTypeConfiguration<Domain.ClienteAggregate.Cliente>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Domain.ClienteAggregate.Cliente> builder)
        {
            builder.Property(e => e.Nome).IsRequired();
            builder.Property(e => e.CartaoCreditoPadraoId).IsRequired();
            builder.HasIndex(e => e.CartaoCreditoPadraoId);
        }
    }
}