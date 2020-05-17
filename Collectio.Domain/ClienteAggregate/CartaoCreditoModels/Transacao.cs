using Collectio.Domain.Base;
using Collectio.Domain.ClienteAggregate.Events;
using Collectio.Domain.ClienteAggregate.Exceptions;
using System;

namespace Collectio.Domain.ClienteAggregate.CartaoCreditoModels
{
    public class Transacao : BaseTenantEntity
    {
        private decimal _valor;
        private StatusTransacaoCartaoValueObject _status;
        private string _cobrancaId;
        private Guid _cartaoId;
        private CartaoCredito _cartaoCredito;
        private string _externalTenantId;

        public string CobrancaId => _cobrancaId;
        public string ExternalTenantId => _externalTenantId;
        public decimal Valor => _valor;
        public Guid CartaoId => _cartaoId;
        public virtual CartaoCredito CartaoCredito => _cartaoCredito;

        public StatusTransacaoCartaoValueObject Status => _status;

        public Transacao(string cobrancaId, Guid cartaoId, decimal valor)
        {
            _cobrancaId = cobrancaId;
            _cartaoId = cartaoId;
            _valor = valor;
            _status = StatusTransacaoCartaoValueObject.Processando();
            AddEvent(new TransacaoCartaoCriadaEvent(Id.ToString()));
        }

        private Transacao(string cobrancaId, decimal valor, StatusTransacaoCartaoValueObject statusTransacao, Transacao transacaoAnterior)
        {
            _cobrancaId = cobrancaId;
            _valor = valor;
            _status = statusTransacao;
            AddEvent(new ReprocessandoTransacaoCartaoEvent(this, transacaoAnterior));
        }

        public Transacao Aprovar(string externalTenantId)
        {
            _status.Aprovar();
            _externalTenantId = externalTenantId;
            AddEvent(new TransacaoCartaoAprovadaEvent(this));
            return this;
        }

        public Transacao DefinirErro(string mensagemErro, string externalTenantId)
        {
            _status.DefinirErro(mensagemErro);
            _externalTenantId = externalTenantId;
            AddEvent(new ErroTransacaoCartaoEvent(this));
            return this;
        }

        public Transacao Reprocessar(decimal valor) 
            => new Transacao(CobrancaId, valor, Status.Reprocessar(), this);

        public class StatusTransacaoCartaoValueObject
        {
            private StatusTransacaoCartao _status;
            private string _mensagemErro;

            public string MensagemErro => _mensagemErro;
            public StatusTransacaoCartao Status => _status;

            private StatusTransacaoCartaoValueObject() 
                => _status = StatusTransacaoCartao.Procesando;

            internal static StatusTransacaoCartaoValueObject Processando()
                => new StatusTransacaoCartaoValueObject();

            internal StatusTransacaoCartaoValueObject Reprocessar()
            {
                if (Status != StatusTransacaoCartao.Erro)
                    throw new ImpossivelReprocessarTransacaoException();

                return new StatusTransacaoCartaoValueObject();
            }

            internal StatusTransacaoCartaoValueObject Aprovar()
            {
                if (Status != StatusTransacaoCartao.Procesando)
                    throw new ImpossivelAprovarTransacaoException();

                _status = StatusTransacaoCartao.Aprovada;
                return this;
            }

            internal StatusTransacaoCartaoValueObject DefinirErro(string mensagemErro)
            {
                if (Status != StatusTransacaoCartao.Procesando)
                    throw new ImpossivelDefinirErroTransacaoException();

                _status = StatusTransacaoCartao.Erro;
                _mensagemErro = mensagemErro;
                return this;
            }
        }
    }
}