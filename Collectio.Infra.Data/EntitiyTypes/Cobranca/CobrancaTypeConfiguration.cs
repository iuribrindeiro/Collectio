using Collectio.Domain.CobrancaAggregate.Pagamentos;
using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.Cobranca
{
    public class CobrancaTypeConfiguration : BaseTenantEntityTypeConfiguration<Domain.CobrancaAggregate.Cobranca>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Domain.CobrancaAggregate.Cobranca> builder)
        {
            builder.Property(c => c.Descricao).IsRequired();
            builder.Property(c => c.Valor).IsRequired();
            builder.Property(c => c.Vencimento).IsRequired();
            builder.Property(c => c.Vencimento).IsRequired();
            builder.Property(c => c.ConfiguracaoEmissaoId).IsRequired();
            builder.HasIndex(c => c.ConfiguracaoEmissaoId);
            builder.HasOne(c => c.Pagamento).WithOne().HasForeignKey<Pagamento>(p => p.CobrancaId).IsRequired();
            builder.HasOne(c => c.Cliente).WithOne(c => c.Cobranca).HasForeignKey<Domain.CobrancaAggregate.Cliente>(c => c.CobrancaId).IsRequired();
            builder.OwnsOne(c => c.Transacao, t =>
            {
                t.Property(p => p.Status).IsRequired();
                t.Property(p => p.FormaPagamento).IsRequired();
            });
            builder.Ignore(c => c.Status);
            builder.Ignore(c => c.FormaPagamentoCartao);
            builder.Ignore(c => c.FormaPagamentoBoleto);
        }
    }
}