using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class FormaPagamentoAguardandoInicioProcessamentoException : BusinessRulesException
    {
        public FormaPagamentoAguardandoInicioProcessamentoException() : base("A forma de pagamento ainda está aguardando o inicio do processamento")
        {
        }
    }
}