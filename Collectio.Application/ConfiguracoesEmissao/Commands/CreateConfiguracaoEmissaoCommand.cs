using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Profiles;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using Collectio.Application.Base.CommandValidators;
using FluentValidation;

namespace Collectio.Application.ConfiguracoesEmissao.Commands
{
    public class CreateConfiguracaoEmissaoCommand : ICommand<string>, IMapTo<ConfiguracaoEmissao>
    {
        public string NomeEmpresa { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Ddd { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateConfiguracaoEmissaoCommand, ConfiguracaoEmissao>()
                .ConstructUsing(c => new ConfiguracaoEmissao(c.NomeEmpresa, c.Agencia, c.Conta, c.CpfCnpj, c.Email, c.Telefone, c.Ddd));
        }
    }

    public class CreateConfiguracaoEmissaoCommandValidator : AbstractValidator<CreateConfiguracaoEmissaoCommand>
    {
        public CreateConfiguracaoEmissaoCommandValidator()
        {
            RuleFor(c => c.NomeEmpresa).NotEmpty();
            RuleFor(c => c.Agencia).NotEmpty().MaximumLength(6);
            RuleFor(c => c.Conta).NotEmpty().MaximumLength(20);
            RuleFor(c => c.CpfCnpj).NotEmpty().IsValidCpfOrCnpj();
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
            RuleFor(c => c.Telefone).NotEmpty().IsValidTelefone();
            RuleFor(c => c.Ddd).NotEmpty().IsValidDdd();
        }
    }
}
