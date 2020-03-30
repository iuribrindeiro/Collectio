using Collectio.Domain.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.Base
{
    public abstract class BaseTenantEntityTypeConfiguration<T> : BaseEntityTypeConfiguration<T> where T : BaseTenantEntity
    {
        protected bool IsUnique = false;

        private void ConfigureBaseTenantEntity(EntityTypeBuilder<T> builder)
        {
            builder.HasIndex(e => e.TenantId).IsUnique(IsUnique);
            builder.Property(e => e.TenantId).HasField("_tenantId");
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            ConfigureBaseTenantEntity(builder);
        }
    }
}