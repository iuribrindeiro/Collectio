using Collectio.Domain.ClienteAggregate.Exceptions;

namespace Collectio.Domain.CartaoCreditoAggregate
{
    public class StatusTransacaoCartaoValueObject
    {
        public string MensagemErro { get; private set; }
        public StatusTransacaoCartao Status { get; private set; }

        private StatusTransacaoCartaoValueObject() {}

        public static StatusTransacaoCartaoValueObject Processando()
            => new StatusTransacaoCartaoValueObject() { Status = StatusTransacaoCartao.Procesando };

        public StatusTransacaoCartaoValueObject Reprocessar()
        {
            if (Status != StatusTransacaoCartao.Erro)
                throw new ImpossivelReprocessarTransacaoException();

            return new StatusTransacaoCartaoValueObject();
        }

        public StatusTransacaoCartaoValueObject Aprovar()
        {
            if (Status != StatusTransacaoCartao.Procesando)
                throw new ImpossivelAprovarTransacaoException();

            Status = StatusTransacaoCartao.Aprovada;
            return this;
        }

        public StatusTransacaoCartaoValueObject DefinirErro(string mensagemErro)
        {
            if (Status != StatusTransacaoCartao.Procesando)
                throw new ImpossivelDefinirErroTransacaoException();

            Status = StatusTransacaoCartao.Erro;
            MensagemErro = mensagemErro;
            return this;
        }
    }
}