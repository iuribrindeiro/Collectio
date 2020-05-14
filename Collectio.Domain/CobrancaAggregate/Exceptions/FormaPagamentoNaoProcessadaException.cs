using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class FormaPagamentoNaoProcessadaException : BusinessRulesException
    {
        public FormaPagamentoNaoProcessadaException() : base("A forma de pagamento ainda não foi processada")
        {
        }
    }
}