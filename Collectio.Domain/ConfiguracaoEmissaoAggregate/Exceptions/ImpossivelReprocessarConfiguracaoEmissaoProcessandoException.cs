using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions
{
    public class ImpossivelReprocessarConfiguracaoEmissaoProcessandoException : BusinessRulesException
    {
        public ImpossivelReprocessarConfiguracaoEmissaoProcessandoException() : base("Não é possível reprocessar uma configuração de recebimento quando já está em processamento")
        {
        }
    }
}