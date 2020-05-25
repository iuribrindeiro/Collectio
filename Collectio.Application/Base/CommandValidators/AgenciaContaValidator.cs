using Collectio.Application.Base.ViewModels;
using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public class AgenciaContaValidator : AbstractValidator<AgenciaContaViewModel>
    {
        public AgenciaContaValidator()
        {
            RuleFor(a => a.Agencia).NotEmpty().IsValidAgenciaBancaria();
            RuleFor(a => a.Conta).NotEmpty().IsValidContaBancaria();
        }
    }
}