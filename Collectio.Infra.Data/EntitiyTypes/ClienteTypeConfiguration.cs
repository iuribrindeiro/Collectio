using Collectio.Domain.ClienteAggregate;
using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes
{
    public class ClienteTypeConfiguration : BaseTenantEntityTypeConfiguration<Cliente>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Cliente> builder)
        {
            builder.Property(e => e.Nome).IsRequired();
        }
    }
}