using Collectio.Domain.Base.Exceptions;

namespace Collectio.Domain.ClienteAggregate.Exceptions
{
    public class CartaoCreditoNaoCadastradoException : BusinessRulesException
    {
        public CartaoCreditoNaoCadastradoException() : base("Cartão de Crédito não cadastrado")
        {
        }
    }

    public class TransacaoCartaoNaoExisteException : BusinessRulesException
    {
        public TransacaoCartaoNaoExisteException() : base("Cartão de Crédito não cadastrado")
        {
        }
    }
}