using Collectio.Domain.Base.Validators;
using Collectio.Domain.CobrancaAggregate.Endereco;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public class EntidadeValidator<T> : AbstractValidator<T>
        where T : Entidade
    {
        public EntidadeValidator()
        {
            RuleFor(e => e.Nome).NotEmpty().NotNull();
            RuleFor(e => e.CpfCnpj).NotNull().IsValid();
            RuleFor(e => e.Endereco).NotNull().SetValidator(new EnderecoValidator());
        }
    }
}