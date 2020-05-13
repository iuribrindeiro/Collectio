using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ProcessoFormaPagamentoJaIniciadoException : BusinessRulesException
    {
        public ProcessoFormaPagamentoJaIniciadoException() : base("Forma de pagamento já está em processamento")
        {
        }
    }
}