using Collectio.Domain.ClienteAggregate.Exceptions;

namespace Collectio.Domain.ClienteAggregate.CartaoCreditoModels
{
    public class StatusCartaoValueObject
    {
        private StatusCartao _status;
        private string _mensagemErro;

        public StatusCartao Status => _status;
        public string MensagemErro => _mensagemErro;

        private StatusCartaoValueObject()
            => _status = StatusCartao.Processando;

        public static StatusCartaoValueObject Processando()
            => new StatusCartaoValueObject();

        public void Processado()
        {
            if (Status != StatusCartao.Processando)
                throw new ImpossivelDefinirStatusCartaoComoProcessadoException();

            _status = StatusCartao.Processado;
        }

        public void Erro(string mensagemErro)
        {
            if (Status != StatusCartao.Processando)
                throw new ImpossivelDefinirStatusCartaoComoErroException();

            _mensagemErro = mensagemErro;
            _status = StatusCartao.Erro;
        }
    }
}