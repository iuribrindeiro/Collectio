using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ClienteAggregate.Exceptions
{
    public class ImpossivelReprocessarTransacaoException : BusinessRulesException
    {
        public ImpossivelReprocessarTransacaoException() : base("Somente transações com erro podem ser reprocessadas")
        {
        }
    }
}