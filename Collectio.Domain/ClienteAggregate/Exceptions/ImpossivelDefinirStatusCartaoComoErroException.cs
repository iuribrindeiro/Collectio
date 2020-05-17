using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ClienteAggregate.Exceptions
{
    public class ImpossivelDefinirStatusCartaoComoErroException : BusinessRulesException
    {
        public ImpossivelDefinirStatusCartaoComoErroException() : base("Somente cartões em processamento podem ser definidos como erro")
        {
        }
    }
}