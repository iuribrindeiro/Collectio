using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class FormaPagamentoAindaEmProcessamentoException : BusinessRulesException
    {
        public FormaPagamentoAindaEmProcessamentoException(FormaPagamentoValueObject formaPagamentoAtual) : base($"A forma de pagamento atual ({formaPagamentoAtual}) ainda está sendo processada. Aguarde até que o processamento seja concluído para alterá-la")
        {
        }
    }
}