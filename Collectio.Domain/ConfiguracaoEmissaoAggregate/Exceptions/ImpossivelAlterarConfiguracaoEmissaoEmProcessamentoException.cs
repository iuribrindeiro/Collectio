using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions
{
    public class ImpossivelAlterarConfiguracaoEmissaoEmProcessamentoException : BusinessRulesException
    {
        public ImpossivelAlterarConfiguracaoEmissaoEmProcessamentoException() : base("Não é possível alterar uma configuração de emissão quando ainda está em processamento")
        {
        }
    }
}