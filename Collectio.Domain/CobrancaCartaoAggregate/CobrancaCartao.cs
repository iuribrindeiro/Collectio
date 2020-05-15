using Collectio.Domain.Base;
using Collectio.Domain.CobrancaCartaoAggregate.Events;

namespace Collectio.Domain.CobrancaCartaoAggregate
{
    public class CobrancaCartao : BaseTenantEntity, IAggregateRoot
    {
        private string _transacaoId;
        private decimal _valor;
        private CartaoValueObject _cartao;
        private string _pagadorId;
        private string _emissorId;
        private StatusCobrancaCartaoValueObject _status;

        public string TransacaoId => _transacaoId;
        public string EmissorId => _emissorId;
        public string PagadorId => _pagadorId;
        public decimal Valor => _valor;
        public CartaoValueObject Cartao => _cartao;
        public StatusCobrancaCartaoValueObject Status => _status;

        public CobrancaCartao(string emissorId, string pagadorId, decimal valor, CartaoValueObject cartao)
        {
            _emissorId = emissorId;
            _pagadorId = pagadorId;
            _valor = valor;
            _cartao = cartao;

            AddEvent(new CobrancaCartaoCriadaEvent(this));
        }

        public class StatusCobrancaCartaoValueObject
        {
            private StatusCobrancaCartao _status;
            private string _motivoErro;

            public string MotivoErro => _motivoErro;
            public StatusCobrancaCartao Status => _status;

            public StatusCobrancaCartaoValueObject()
            {
                _status = StatusCobrancaCartao.Procesando;
            }

            internal void DefinirComoPago()
                => _status = StatusCobrancaCartao.Pago;

            internal void DefinirComoErro(string motivoErro)
            {
                _status = StatusCobrancaCartao.Erro;
                _motivoErro = motivoErro;
            }
        }
    }

    public enum StatusCobrancaCartao
    {
        Procesando,
        Pago,
        Erro
    }
}
