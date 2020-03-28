using Collectio.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.Base
{
    public abstract class BaseTenantEntityTypeConfiguration<T> : BaseEntityTypeConfiguration<T> where T : BaseTenantEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder, bool unique = false)
        {
            builder
                .OwnsOne(e => e.TenantId, eb => 
                {
                    eb.Property(e => e.Value).HasField("_value");
                })
                .Property(e => e.TenantId)
                .HasField("_tenantId")
                .HasColumnName("TenantId")
                .IsRequired()
                .IsUnicode(unique);
            base.Configure(builder);
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            Configure(builder, false);
        }
    }
}