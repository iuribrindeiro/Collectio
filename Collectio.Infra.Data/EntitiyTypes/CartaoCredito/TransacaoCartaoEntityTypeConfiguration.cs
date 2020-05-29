using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.CartaoCredito
{
    public class TransacaoCartaoEntityTypeConfiguration : BaseOwnerEntityTypeConfiguration<Transacao>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Transacao> builder)
        {
            builder.Property(t => t.Valor).IsRequired();
            builder.HasIndex(t => t.CobrancaId);
            builder.Property(t => t.CobrancaId).IsRequired();
            builder.HasOne(t => t.CartaoCredito).WithMany(c => c.Transacoes).HasForeignKey(t => t.CartaoId);
            builder.OwnsOne(t => t.Status, p =>
            {
                p.Property(s => s.Status).IsRequired();
                p.Property(s => s.MensagemErro).IsRequired(false);
            });
        }
    }
}