using Collectio.Domain.Base.Validators;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.ContaBancarias
{
    public class ContaBancariaValidator : AbstractValidator<ContaBancaria>
    {
        public ContaBancariaValidator()
        {
            RuleFor(c => c.Descricao).NotEmpty();
            RuleFor(c => c.Banco).NotEmpty();
            RuleFor(c => c.AgenciaConta).IsValid();
        }
    }
}