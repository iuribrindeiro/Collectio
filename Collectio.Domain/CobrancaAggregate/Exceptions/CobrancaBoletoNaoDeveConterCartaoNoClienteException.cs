using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class CobrancaBoletoNaoDeveConterCartaoNoClienteException : BusinessRulesException
    {
        public CobrancaBoletoNaoDeveConterCartaoNoClienteException() : base("Não é possível vincular um cartão de crédito a um cliente cuja forma de pagamento da cobrança é boleto")
        {
        }
    }
}