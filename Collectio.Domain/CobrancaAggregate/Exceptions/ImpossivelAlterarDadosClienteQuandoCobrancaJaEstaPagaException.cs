using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ImpossivelAlterarDadosClienteQuandoCobrancaJaEstaPagaException : BusinessRulesException
    {
        public ImpossivelAlterarDadosClienteQuandoCobrancaJaEstaPagaException() : base("Não é possível atualizar os dados de um cliente cuja cobrança já está paga")
        {
        }
    }
}