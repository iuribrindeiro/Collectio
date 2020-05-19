using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions
{
    public class ImpossivelDefinirErroConfiguracaoRecebimentoException : BusinessRulesException
    {
        public ImpossivelDefinirErroConfiguracaoRecebimentoException() : base("Só é possível definir erro na configuração de recebimento quando está processando")
        {
        }
    }
}