using Collectio.Domain.ClienteAggregate.Exceptions;

namespace Collectio.Domain.CartaoCreditoAggregate
{
    public class StatusCartaoValueObject
    {
        public StatusCartao Status { get; private set; }
        public string MensagemErro { get; private set; }

        private StatusCartaoValueObject() {}

        public static StatusCartaoValueObject Processando()
            => new StatusCartaoValueObject() { Status = StatusCartao.Processando};

        public void Processado()
        {
            if (Status != StatusCartao.Processando)
                throw new ImpossivelDefinirStatusCartaoComoProcessadoException();

            Status = StatusCartao.Processado;
        }

        public void Erro(string mensagemErro)
        {
            if (Status != StatusCartao.Processando)
                throw new ImpossivelDefinirStatusCartaoComoErroException();

            MensagemErro = mensagemErro;
            Status = StatusCartao.Erro;
        }
    }
}