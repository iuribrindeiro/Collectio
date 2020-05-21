using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.CobrancaAggregate.Exceptions
{
    public class SomenteCobrancasCartaoDevemTerCartaoVinculadoException : BusinessRulesException
    {
        public SomenteCobrancasCartaoDevemTerCartaoVinculadoException() : base("Não é possível adicionar um cartão de crédito a uma cobrança de boleto")
        {
        }
    }
}