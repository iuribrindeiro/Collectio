using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.CartaoCredito
{
    public class CartaoCreditoEntityTypeConfiguration : BaseOwnerEntityTypeConfiguration<Domain.CartaoCreditoAggregate.CartaoCredito>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Domain.CartaoCreditoAggregate.CartaoCredito> builder)
        {
            builder.Property(c => c.Numero).IsRequired();
            builder.Property(c => c.Nome).IsRequired();
            builder.HasIndex(c => c.CpfCnpjProprietario);
            builder.Property(c => c.CpfCnpjProprietario).IsRequired();
            builder.OwnsOne(c => c.Status, p =>
            {
                p.Property(s => s.MensagemErro).IsRequired(false);
                p.Property(s => s.Status).IsRequired();
                p.Seed();
            });
            builder.HasMany(c => c.Transacoes).WithOne(t => t.CartaoCredito).HasForeignKey(t => t.CartaoId);

            builder.Seed();
        }
    }
}
