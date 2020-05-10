using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.AjustesValorPagamento
{
    public class Desconto : AjusteValorPagamento
    {
        public Desconto(decimal valor, PeriodoAjuste periodo) : base(valor, periodo)
        {
        }

        protected override IValidator ValidatorFactory() 
            => new AjusteValorPagamentoValidator<Desconto>();
    }
}