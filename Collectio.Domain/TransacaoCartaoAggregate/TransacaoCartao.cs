using Collectio.Domain.Base;
using Collectio.Domain.TransacaoCartaoAggregate.Events;
using Collectio.Domain.TransacaoCartaoAggregate.Exceptions;

namespace Collectio.Domain.TransacaoCartaoAggregate
{
    public sealed class TransacaoCartao : BaseTenantEntity, IAggregateRoot
    {
        private decimal _valor;
        private CartaoValueObject _cartao;
        private string _pagadorId;
        private string _emissorId;
        private StatusTransacaoCartaoValueObject _status;
        private string _idCobranca;

        public string IdCobranca => _idCobranca;
        public string EmissorId => _emissorId;
        public string PagadorId => _pagadorId;
        public decimal Valor => _valor;
        public CartaoValueObject Cartao => _cartao;
        public StatusTransacaoCartaoValueObject Status => _status;

        public TransacaoCartao(string idCobranca, string emissorId, string pagadorId, decimal valor, CartaoValueObject cartao)
        {
            _idCobranca = idCobranca;
            _emissorId = emissorId;
            _pagadorId = pagadorId;
            _valor = valor;
            _cartao = cartao;
            _status = StatusTransacaoCartaoValueObject.CriandoTokenCartao();

            AddEvent(new TransacaoCartaoCriadaEvent(this));
        }

        private TransacaoCartao(string idCobranca, string emissorId, string pagadorId, decimal valor, StatusTransacaoCartaoValueObject statusTransacao, TransacaoCartao transacaoCartaoAnterior)
        {
            _idCobranca = idCobranca;
            _emissorId = emissorId;
            _pagadorId = pagadorId;
            _valor = valor;
            _status = statusTransacao;
            AddEvent(new ReprocessandoTransacaoCartaoEvent(this, transacaoCartaoAnterior));
        }

        public TransacaoCartao Processando(string tokenCartao)
        {
            _status.Processando(tokenCartao);
            AddEvent(new ProcessandoTransacaoCartaoEvent(this));
            return this;
        }

        public TransacaoCartao Aprovar(string transacaoId)
        {
            _status.Aprovar(transacaoId);
            AddEvent(new TransacaoCartaoAprovadaEvent(this));
            return this;
        }

        public TransacaoCartao DefinirErro(string mensagemErro, string transacaoId)
        {
            _status.DefinirErro(mensagemErro, transacaoId);
            AddEvent(new ErroTransacaoCartaoEvent(this));
            return this;
        }

        public TransacaoCartao Reprocessar(string emissorId, string pagadorId, decimal valor) 
            => new TransacaoCartao(IdCobranca, emissorId, pagadorId, valor, Status.Reprocessar(), this);

        public class StatusTransacaoCartaoValueObject
        {
            private StatusTransacaoCartao _status;
            private string _mensagemErro;
            private string _transacaoId;
            private string _tokenCartao;

            public string TokenCartao => _tokenCartao;
            public string MensagemErro => _mensagemErro;
            public string TransacaoId => _transacaoId;
            public StatusTransacaoCartao Status => _status;

            internal static StatusTransacaoCartaoValueObject CriandoTokenCartao() 
                => new StatusTransacaoCartaoValueObject();

            private StatusTransacaoCartaoValueObject()
            {
                _status = StatusTransacaoCartao.CriandoTokenCartao;
            }

            private StatusTransacaoCartaoValueObject(string tokenCartao)
            {
                _status = StatusTransacaoCartao.Procesando;
                _tokenCartao = tokenCartao;
            }

            internal StatusTransacaoCartaoValueObject Reprocessar()
            {
                if (Status != StatusTransacaoCartao.Erro)
                    throw new ImpossivelReprocessarTransacaoException();

                return new StatusTransacaoCartaoValueObject(TokenCartao);
            }

            internal StatusTransacaoCartaoValueObject Processando(string tokenCartao)
            {
                if (Status != StatusTransacaoCartao.CriandoTokenCartao)
                    throw new ImpossivelIniciarProcessamentoTransacaoException();

                _tokenCartao = tokenCartao;
                _status = StatusTransacaoCartao.Procesando;
                return this;
            }

            internal StatusTransacaoCartaoValueObject Aprovar(string transacaoId)
            {
                if (Status != StatusTransacaoCartao.Procesando)
                    throw new ImpossivelAprovarTransacaoException();

                _transacaoId = transacaoId;
                _status = StatusTransacaoCartao.Aprovada;
                return this;
            }

            internal StatusTransacaoCartaoValueObject DefinirErro(string mensagemErro, string transacaoId)
            {
                if (Status != StatusTransacaoCartao.Procesando)
                    throw new ImpossivelDefinirErroTransacaoException();

                _status = StatusTransacaoCartao.Erro;
                _mensagemErro = mensagemErro;
                _transacaoId = transacaoId;
                return this;
            }
        }
    }

    public enum StatusTransacaoCartao
    {
        CriandoTokenCartao,
        Procesando,
        Aprovada,
        Erro
    }
}
