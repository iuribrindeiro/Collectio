using Collectio.Domain.Base.Validators;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public class EntidadeValidator<T> : AbstractValidator<T>
        where T : IEntidade
    {
        public EntidadeValidator()
        {
            RuleFor(e => e.Nome).NotEmpty().NotNull();
            RuleFor(e => e.CpfCnpj).NotNull().IsValid();
            RuleFor(e => e.Endereco).NotNull().SetValidator(new EnderecoValidator());
        }
    }
}