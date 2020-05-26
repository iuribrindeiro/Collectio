using Collectio.Application.Base.CommandValidators;
using Collectio.Application.Cobrancas.ViewModels;
using FluentValidation;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public class ClienteCobrancaValidator : AbstractValidator<ClienteCobrancaViewModel>
    {
        public ClienteCobrancaValidator()
        {
            RuleFor(c => c.TenantId).NotEmpty();
            RuleFor(c => c.CpfCnpj).NotEmpty().IsValidCpfOrCnpj();
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
            RuleFor(c => c.Endereco).NotNull().WithMessage("O endereço do cliente deve ser informado").SetValidator(new EnderecoValidator());
            RuleFor(c => c.Nome).NotEmpty();
            RuleFor(c => c.Telefone).NotNull().WithMessage("O telefone do cliente deve ser informado").SetValidator(new TelefoneValidator());
        }
    }
}