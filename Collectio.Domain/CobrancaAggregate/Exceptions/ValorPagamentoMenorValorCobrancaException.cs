using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class ValorPagamentoMenorValorCobrancaException : BusinessRulesException
    {
        public ValorPagamentoMenorValorCobrancaException(decimal valorCobranca, decimal valorPagamento) 
            : base($"O valor do pagamento {valorPagamento} é menor que o valor da cobranca {valorCobranca}")
        {
        }
    }
}