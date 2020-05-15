using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ImpossivelAlterarCobrancaPagaException : BusinessRulesException
    {
        public ImpossivelAlterarCobrancaPagaException() : base("Impossível alterar cobrança com pagamento realizado")
        {
        }
    }
}