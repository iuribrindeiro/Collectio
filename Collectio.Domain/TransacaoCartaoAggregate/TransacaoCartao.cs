using Collectio.Domain.Base;
using Collectio.Domain.TransacaoCartaoAggregate.Events;
using System;

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
            _status = StatusTransacaoCartaoValueObject.Processando();

            AddEvent(new TransacaoCartaoCriadaEvent(this));
        }

        public TransacaoCartao AprovarTransacao(string transacaoId)
        {
            _status.Aprovar(transacaoId);
            AddEvent(new TransacaoCartaoAprovadaEvent(this));
            return this;
        }

        public TransacaoCartao ErroTransacao(string mensagemErro)
        {
            _status.DefinirErro(mensagemErro);
            AddEvent(new ErroTransacaoCartaoEvent(this));
            return this;
        }

        public class StatusTransacaoCartaoValueObject
        {
            private StatusTransacaoCartao _status;
            private string _mensagemErro;
            private string _transacaoId;

            public string MensagemErro => _mensagemErro;
            public string TransacaoId => _transacaoId;
            public StatusTransacaoCartao Status => _status;

            internal static StatusTransacaoCartaoValueObject Processando() 
                => new StatusTransacaoCartaoValueObject();

            private StatusTransacaoCartaoValueObject()
            {
                _status = StatusTransacaoCartao.Procesando;
            }

            internal void Aprovar(string transacaoId)
            {
                ValidarAlteracaoStatusFinalizado();
                _transacaoId = transacaoId;
                _status = StatusTransacaoCartao.Aprovada;
            }

            internal void DefinirErro(string mensagemErro)
            {
                ValidarAlteracaoStatusFinalizado();
                _status = StatusTransacaoCartao.Erro;
                _mensagemErro = mensagemErro;
            }

            private void ValidarAlteracaoStatusFinalizado()
            {
                if (Status == StatusTransacaoCartao.Aprovada)
                    throw new Exception("Transacao já finalizada");

                if (Status == StatusTransacaoCartao.Erro)
                    throw new Exception("Transacao já definida como erro");
            }
        }
    }

    public enum StatusTransacaoCartao
    {
        Procesando,
        Aprovada,
        Erro
    }
}
