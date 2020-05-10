using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class Multa : AjusteValorPagamento
    {
        public Multa(decimal valor, PeriodoAjuste periodo) : base(valor, periodo)
        {
        }

        protected override IValidator ValidatorFactory()
            => new AjusteValorPagamentoValidator<Multa>();
    }
}