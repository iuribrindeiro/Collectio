using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.Cobranca
{
    public class ClienteTypeConfiguration : BaseTenantEntityTypeConfiguration<Domain.CobrancaAggregate.Cliente>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Domain.CobrancaAggregate.Cliente> builder)
        {
            builder.ToTable("ClienteCobranca");
            builder.Property(c => c.Nome).IsRequired();
            builder.Property(c => c.Email).IsRequired();
            builder.Property(c => c.CpfCnpj).IsRequired();
            builder.Property(c => c.TenantId).IsRequired();
            builder.HasIndex(c => c.TenantId);
            builder.OwnsOne(c => c.CartaoCredito, c =>
            {
                c.Property(p => p.TenantId).IsRequired(false);
                c.Property(p => p.Nome).IsRequired();
                c.Property(p => p.Numero).IsRequired();
            });
            builder.OwnsOne(c => c.Endereco, e =>
            {
                e.Property(p => p.Numero).IsRequired();
                e.Property(p => p.Bairro).IsRequired();
                e.Property(p => p.Cep).IsRequired();
                e.Property(p => p.Rua).IsRequired();
                e.Property(p => p.Cidade).IsRequired();
                e.Property(p => p.Uf).IsRequired();
            });
            builder.OwnsOne(c => c.Telefone, e =>
            {
                e.Property(p => p.Numero).IsRequired();
                e.Property(p => p.Ddd).IsRequired();
            });
        }
    }
}