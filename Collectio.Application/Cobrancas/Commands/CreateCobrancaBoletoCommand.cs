using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Cobrancas.CommandValidators;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;

namespace Collectio.Application.Cobrancas.Commands
{
    public class CreateCobrancaBoletoCommand : BaseCreateCobrancaCommand, ICommand<CobrancaViewModel>, IMapping
    {
        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCobrancaBoletoCommand, Cobranca>()
                .ConstructUsing((c, context) => Cobranca.Boleto(
                    c.Descricao, c.Valor, c.Vencimento, c.ConfiguracaoEmissorId, 
                    c.Cliente.Nome, c.Cliente.CpfCnpj, c.Cliente.Email,
                    context.Mapper.Map<Telefone>(c.Cliente.Telefone), 
                    context.Mapper.Map<Endereco>(c.Cliente.Endereco), c.Cliente.TenantId));
    }

    public class CreateCobrancaBoletoValidator : BaseCreateCobrancaCommandValidator<CreateCobrancaBoletoCommand>
    {
        public CreateCobrancaBoletoValidator(IConfiguracaoEmissaoRepository configuracaoEmissaoRepository) : base(configuracaoEmissaoRepository)
        {
        }
    }
}
