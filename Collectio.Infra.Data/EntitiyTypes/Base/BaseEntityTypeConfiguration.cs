using Collectio.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.Base
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasField("_id");
            builder.Property(e => e.DataAtualizacao).HasField("_dataAtualizacao");
            builder.Property(e => e.DataCriacao).HasField("_dataCriacao");
        }
    }
}
