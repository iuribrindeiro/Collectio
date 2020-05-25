using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Base.CommandValidators;
using Collectio.Application.Base.ViewModels;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Collectio.Application.ConfiguracoesEmissao.Commands
{
    public class CreateConfiguracaoEmissaoCommand : ICommand<string>, IMapping
    {
        [Required]
        public string NomeEmpresa { get; set; }

        [Required]
        public AgenciaContaViewModel AgenciaConta { get; set; }
        
        [Required]
        public string CpfCnpj { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public TelefoneViewModel Telefone { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateConfiguracaoEmissaoCommand, ConfiguracaoEmissao>()
                .ConstructUsing((c, context) 
                    => new ConfiguracaoEmissao(c.NomeEmpresa, c.CpfCnpj, c.Email, context.Mapper.Map<AgenciaConta>(c.AgenciaConta),  context.Mapper.Map<Telefone>(c.Telefone)));
        }
    }

    public class CreateConfiguracaoEmissaoCommandValidator : AbstractValidator<CreateConfiguracaoEmissaoCommand>
    {
        public CreateConfiguracaoEmissaoCommandValidator()
        {
            RuleFor(c => c.NomeEmpresa).NotEmpty().WithName("Nome da Empresa");
            RuleFor(c => c.AgenciaConta).NotEmpty().WithName("Agencia/Conta").WithMessage("Informe a {PropertyName}").SetValidator(new AgenciaContaValidator());
            RuleFor(c => c.CpfCnpj).NotEmpty().IsValidCpfOrCnpj().WithName("CPF/CNPJ");
            RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("'{PropertyValue}' não é um email válido inválido");
            RuleFor(c => c.Telefone).NotEmpty().SetValidator(new TelefoneValidator());
        }
    }
}
