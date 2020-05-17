using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ClienteAggregate.Exceptions
{
    public class CartaoCreditoNaoProcessadoException : BusinessRulesException
    {
        public CartaoCreditoNaoProcessadoException() : base("O cartão de crédito escolhido ainda não está disponível para realizar transações")
        {
        }
    }
}