using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.TransacaoCartaoAggregate.Exceptions
{
    public class ImpossivelDefinirErroTransacaoException : BusinessRulesException
    {
        public ImpossivelDefinirErroTransacaoException() : base("Somente transações processando podem ser definidas como erro")
        {
        }
    }
}