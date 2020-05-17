using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ClienteAggregate.Exceptions
{
    public class ImpossivelDefinirErroTransacaoException : BusinessRulesException
    {
        public ImpossivelDefinirErroTransacaoException() : base("Somente transações processando podem ser definidas como erro")
        {
        }
    }
}