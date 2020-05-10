using Collectio.Domain.CobrancaAggregate.AjustesValorPagamento;
using Collectio.Domain.CobrancaAggregate.ContaBancaria;
using Collectio.Domain.CobrancaAggregate.Entidades;
using Collectio.Domain.CobrancaAggregate.Pagamento;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate
{
    public class CobrancaValidator : AbstractValidator<Cobranca>
    {
        public CobrancaValidator()
        {
            RuleFor(e => e.Valor).NotEmpty();
            RuleFor(e => e.Vencimento).NotEmpty();
            RuleFor(e => e.Pagador).SetValidator(new EntidadeValidator<Pagador>());
            RuleFor(e => e.Emissor).SetValidator(new EntidadeValidator<Emissor>());
            RuleFor(e => e.Pagamento).SetValidator(new PagamentoValidator());
            RuleFor(e => e.ContaBancaria).SetValidator(new ContaBancariaValidator());
            RuleFor(e => e.Juros).SetValidator(new AjusteValorPagamentoValidator<Juros>());
            RuleFor(e => e.Multa).SetValidator(new AjusteValorPagamentoValidator<Multa>());
            RuleFor(e => e.Desconto).SetValidator(new AjusteValorPagamentoValidator<Desconto>());
        }
    }
}