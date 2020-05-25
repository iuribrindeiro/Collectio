using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collectio.Infra.Data.EntitiyTypes.ConfiguracaoEmissao
{
    public class ConfiguracaoEmissaoEntityTypeConfiguration : BaseOwnerEntityTypeConfiguration<Domain.ConfiguracaoEmissaoAggregate.ConfiguracaoEmissao>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Domain.ConfiguracaoEmissaoAggregate.ConfiguracaoEmissao> builder)
        {
            builder.Property(c => c.NomeEmpresa).IsRequired();
            builder.Property(c => c.CpfCnpj).IsRequired();
            builder.Property(c => c.Email).IsRequired();
            builder.OwnsOne(c => c.Telefone, t =>
            {
                t.Property(t => t.Numero).IsRequired();
                t.Property(t => t.Ddd).IsRequired();
            });
            builder.OwnsOne(c => c.Status, s =>
            {
                s.Property(p => p.MensagemErro).IsRequired(false);
                s.Property(p => p.Status).IsRequired();
            });
            builder.OwnsOne(c => c.AgenciaConta, ac =>
            {
                ac.Property(p => p.Agencia).IsRequired();
                ac.Property(p => p.Conta).IsRequired();
            });
        }
    }
}
