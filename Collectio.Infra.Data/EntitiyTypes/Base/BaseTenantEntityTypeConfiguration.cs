using Collectio.Domain.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.Base
{
    public abstract class BaseTenantEntityTypeConfiguration<T> : BaseEntityTypeConfiguration<T> where T : BaseOwnerEntity
    {
        protected bool IsUnique = false;

        private void ConfigureBaseTenantEntity(EntityTypeBuilder<T> builder)
        {
            builder.HasIndex(e => e.OwnerId).IsUnique(IsUnique);
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            ConfigureBaseTenantEntity(builder);
        }
    }
}