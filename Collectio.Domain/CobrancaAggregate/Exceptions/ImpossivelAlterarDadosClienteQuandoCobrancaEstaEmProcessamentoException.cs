using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ImpossivelAlterarDadosClienteQuandoCobrancaEstaEmProcessamentoException : BusinessRulesException
    {
        public ImpossivelAlterarDadosClienteQuandoCobrancaEstaEmProcessamentoException() : base("Não é possível alterar os dados do cliente quando a cobrança ainda está em processamento")
        {
        }
    }
}