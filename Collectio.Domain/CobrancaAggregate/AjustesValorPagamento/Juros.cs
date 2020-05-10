using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class Juros : AjusteValorPagamento
    {
        public Juros(decimal valor, PeriodoAjuste periodo) : base(valor, periodo)
        {
        }

        protected override IValidator ValidatorFactory() 
            => new AjusteValorPagamentoValidator<Juros>();
    }
}