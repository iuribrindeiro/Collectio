using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class CobrancasComCartaoDevemPossuirClienteComCartaoCreditoVinculadoException : BusinessRulesException
    {
        public CobrancasComCartaoDevemPossuirClienteComCartaoCreditoVinculadoException(Cobranca cobranca, Cliente cliente) 
            : base($"O cliente {cliente} da cobranca {cobranca} não possúi cartão de crédito vinculado")
        {
        }
    }
}