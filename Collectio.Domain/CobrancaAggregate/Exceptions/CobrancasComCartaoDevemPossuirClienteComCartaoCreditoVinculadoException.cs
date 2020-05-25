using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class CobrancasComCartaoDevemPossuirClienteComCartaoCreditoVinculadoException : BusinessRulesException
    {
        public CobrancasComCartaoDevemPossuirClienteComCartaoCreditoVinculadoException(Cobranca cobranca, ClienteCobranca clienteCobranca) 
            : base($"O cliente {clienteCobranca} da cobranca {cobranca} não possúi cartão de crédito vinculado")
        {
        }
    }
}