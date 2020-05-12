using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public class EnderecoValidator : AbstractValidator<Endereco>
    {
        public EnderecoValidator()
        {
            RuleFor(e => e.Rua).NotEmpty();
            RuleFor(e => e.Numero).NotEmpty();
            RuleFor(e => e.Bairro).NotEmpty();
            RuleFor(e => e.Cep).NotEmpty();
            RuleFor(e => e.Cidade).NotEmpty();
            RuleFor(e => e.Estado).NotEmpty();
        }
    }
}