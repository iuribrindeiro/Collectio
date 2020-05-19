using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions
{
    public class ImpossivelProcessarConfiguracaoRecebimentoException : BusinessRulesException
    {
        public ImpossivelProcessarConfiguracaoRecebimentoException() : base("Só é possível processar uma configuração de recebimento quando está processando")
        {
        }
    }
}