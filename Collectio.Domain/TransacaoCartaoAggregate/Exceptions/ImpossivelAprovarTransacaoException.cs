using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.TransacaoCartaoAggregate.Exceptions
{
    public class ImpossivelAprovarTransacaoException : BusinessRulesException
    {
        public ImpossivelAprovarTransacaoException() : base("Somente transações processando podem ser aprovadas")
        {
        }
    }
}