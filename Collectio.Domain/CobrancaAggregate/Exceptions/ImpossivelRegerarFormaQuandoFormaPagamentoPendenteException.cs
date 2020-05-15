using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ImpossivelRegerarFormaQuandoFormaPagamentoPendenteException : BusinessRulesException
    {
        public ImpossivelRegerarFormaQuandoFormaPagamentoPendenteException() : base("Não é possível regerar a forma de pagamento quando já existe uma em processamento")
        {
        }
    }
}