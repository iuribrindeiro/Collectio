using System.Runtime.CompilerServices;
using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.CartaoCredito
{
    public class CartaoCreditoEntityTypeConfiguration : BaseOwnerEntityTypeConfiguration<Domain.CartaoCreditoAggregate.CartaoCredito>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Domain.CartaoCreditoAggregate.CartaoCredito> builder)
        {
            builder.Property(c => c.Numero).IsRequired(false);
            builder.Property(c => c.Nome).IsRequired();
            builder.HasIndex(c => c.CpfCnpjProprietario);
            builder.Property(c => c.CpfCnpjProprietario).IsRequired();
            builder.OwnsOne(c => c.Status, p =>
            {
                p.Property(s => s.MensagemErro).IsRequired(false);
                p.Property(s => s.Status).IsRequired();
            });
            builder.HasMany(c => c.Transacoes).WithOne(t => t.CartaoCredito).HasForeignKey(t => t.CartaoId);
        }
    }
}
