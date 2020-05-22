using Collectio.Application.Cobrancas.ViewModels;
using FluentValidation;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public class EnderecoValidator : AbstractValidator<EnderecoViewModel>
    {
        public EnderecoValidator()
        {
            RuleFor(e => e.Numero).NotEmpty();
            RuleFor(e => e.Bairro).NotEmpty();
            RuleFor(e => e.Cep).NotEmpty();
            RuleFor(e => e.Cidade).NotEmpty();
            RuleFor(e => e.Rua).NotEmpty();
            RuleFor(e => e.Uf).NotEmpty();
        }
    }
}