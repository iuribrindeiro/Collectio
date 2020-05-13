using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ProcessoFormaPagamentoJaFinalizadoException : BusinessRulesException
    {
        public ProcessoFormaPagamentoJaFinalizadoException() : base("Forma de pagamento já está processada")
        {
        }
    }
}