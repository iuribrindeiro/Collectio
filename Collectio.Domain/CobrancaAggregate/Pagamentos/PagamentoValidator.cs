using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Pagamentos
{
    public class PagamentoValidator : AbstractValidator<Pagamento>
    {
        public PagamentoValidator()
        {
            RuleFor(e => e.Valor).NotNull();
        }
    }
}