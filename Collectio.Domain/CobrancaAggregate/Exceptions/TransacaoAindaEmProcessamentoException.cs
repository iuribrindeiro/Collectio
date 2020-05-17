using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class TransacaoAindaEmProcessamentoException : BusinessRulesException
    {
        public TransacaoAindaEmProcessamentoException(FormaPagamento formaPagamentoAtual) : base($"A transação atual ({formaPagamentoAtual}) ainda está sendo processada. Aguarde até que o processamento seja concluído para alterá-la")
        {
        }
    }
}