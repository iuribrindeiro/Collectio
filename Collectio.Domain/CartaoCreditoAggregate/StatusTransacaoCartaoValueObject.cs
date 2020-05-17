using Collectio.Domain.ClienteAggregate.Exceptions;

namespace Collectio.Domain.CartaoCreditoAggregate
{
    public class StatusTransacaoCartaoValueObject
    {
        private StatusTransacaoCartao _status;
        private string _mensagemErro;

        public string MensagemErro => _mensagemErro;
        public StatusTransacaoCartao Status => _status;

        private StatusTransacaoCartaoValueObject()
            => _status = StatusTransacaoCartao.Procesando;

        public static StatusTransacaoCartaoValueObject Processando()
            => new StatusTransacaoCartaoValueObject();

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

            _status = StatusTransacaoCartao.Aprovada;
            return this;
        }

        public StatusTransacaoCartaoValueObject DefinirErro(string mensagemErro)
        {
            if (Status != StatusTransacaoCartao.Procesando)
                throw new ImpossivelDefinirErroTransacaoException();

            _status = StatusTransacaoCartao.Erro;
            _mensagemErro = mensagemErro;
            return this;
        }
    }
}