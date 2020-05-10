using Collectio.Domain.Base.Validators;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.ContaBancaria
{
    public class ContaBancariaValidator : AbstractValidator<ContaBancaria>
    {
        public ContaBancariaValidator()
        {
            RuleFor(c => c.Nome).NotEmpty();
            RuleFor(c => c.ContaBanco).IsValid();
        }
    }
}