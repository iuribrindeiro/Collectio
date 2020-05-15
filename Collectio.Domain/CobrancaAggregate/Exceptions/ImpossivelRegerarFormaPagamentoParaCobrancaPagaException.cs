using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ImpossivelRegerarFormaPagamentoParaCobrancaPagaException : BusinessRulesException
    {
        public ImpossivelRegerarFormaPagamentoParaCobrancaPagaException() : base("Não é possível regerar a forma de pagamento para uma cobrança paga")
        {
        }
    }
}