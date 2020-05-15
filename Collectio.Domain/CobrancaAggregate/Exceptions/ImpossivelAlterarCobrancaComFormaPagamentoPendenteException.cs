using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ImpossivelAlterarCobrancaComFormaPagamentoPendenteException : BusinessRulesException
    {
        public ImpossivelAlterarCobrancaComFormaPagamentoPendenteException() : base("Aguarde até que a forma de pagamento seja processada")
        {
        }
    }
}