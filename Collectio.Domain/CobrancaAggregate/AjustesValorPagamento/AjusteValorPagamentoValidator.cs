using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class AjusteValorPagamentoValidator<T> : AbstractValidator<T>
        where T : AjusteValorPagamento
    {
        public AjusteValorPagamentoValidator()
        {
            RuleFor(e => e.Valor).NotEmpty();
            RuleFor(e => e.Periodo).NotNull();
        }
    }
}