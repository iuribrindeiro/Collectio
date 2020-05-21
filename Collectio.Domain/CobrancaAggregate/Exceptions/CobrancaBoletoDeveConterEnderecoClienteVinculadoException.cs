using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class CobrancaBoletoDeveConterEnderecoClienteVinculadoException : BusinessRulesException
    {
        public CobrancaBoletoDeveConterEnderecoClienteVinculadoException() : base("Cobrança de boleto deve ter endereço do cliente vinculado")
        {
        }
    }
}