using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions
{
    public class ImpossivelReprocessarConfiguracaoRecebimentoProcessandoException : BusinessRulesException
    {
        public ImpossivelReprocessarConfiguracaoRecebimentoProcessandoException() : base("Não é possível reprocessar uma configuração de recebimento quando já está em processamento")
        {
        }
    }
}