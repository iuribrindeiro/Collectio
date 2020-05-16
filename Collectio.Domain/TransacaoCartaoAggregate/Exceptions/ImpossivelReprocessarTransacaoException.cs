using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.TransacaoCartaoAggregate.Exceptions
{
    public class ImpossivelReprocessarTransacaoException : BusinessRulesException
    {
        public ImpossivelReprocessarTransacaoException() : base("Somente transações com erro podem ser reprocessadas")
        {
        }
    }
}