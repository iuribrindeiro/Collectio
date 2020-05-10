using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Pagamento
{
    public class PagamentoValidator : AbstractValidator<Pagamento>
    {
        public PagamentoValidator()
        {
            RuleFor(e => e.Valor).NotNull();
        }
    }
}