using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ClienteAggregate.Exceptions
{
    public class ImpossivelDefinirStatusCartaoComoProcessadoException : BusinessRulesException
    {
        public ImpossivelDefinirStatusCartaoComoProcessadoException() : base("Somente cartões em processamento podem ser definidos como processado")
        {
        }
    }
}