using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.TransacaoCartaoAggregate.Exceptions
{
    public class ImpossivelIniciarProcessamentoTransacaoException : BusinessRulesException
    {
        public ImpossivelIniciarProcessamentoTransacaoException() : base("Somente transações com token cartão definido podem iniciar processamento")
        {
        }
    }
}