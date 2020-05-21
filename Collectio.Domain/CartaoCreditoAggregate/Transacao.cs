using System;
using Collectio.Domain.Base;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using Collectio.Domain.ClienteAggregate.Events;
using Collectio.Domain.ClienteAggregate.Exceptions;

namespace Collectio.Domain.CartaoCreditoAggregate
{
    public class Transacao : BaseOwnerEntity
    {
        private decimal _valor;
        private StatusTransacaoCartaoValueObject _status;
        private string _cobrancaId;
        private Guid _cartaoId;
        private CartaoCredito _cartaoCredito;
        private string _contaBancariaId;

        public string CobrancaId => _cobrancaId;
        public decimal Valor => _valor;
        public string ContaBancariaId => _contaBancariaId;
        public Guid CartaoId => _cartaoId;
        public virtual CartaoCredito CartaoCredito => _cartaoCredito;

        public StatusTransacaoCartaoValueObject Status => _status;

        public Transacao(string cobrancaId, string contaBancariaId, CartaoCredito cartaoCredito, decimal valor)
        {
            if (!cartaoCredito.CartaoProcessado)
                throw new CartaoCreditoNaoProcessadoException();

            _cobrancaId = cobrancaId;
            _contaBancariaId = contaBancariaId;
            _cartaoId = cartaoCredito.Id;
            _valor = valor;
            _status = StatusTransacaoCartaoValueObject.Processando();
            AddEvent(new TransacaoCartaoCriadaEvent(Id.ToString()));
        }

        private Transacao(string cobrancaId, string contaBancariaId, decimal valor, StatusTransacaoCartaoValueObject statusTransacao, Transacao transacaoAnterior)
        {
            _cobrancaId = cobrancaId;
            _contaBancariaId = contaBancariaId;
            _valor = valor;
            _status = statusTransacao;
            AddEvent(new ReprocessandoTransacaoCartaoEvent(this, transacaoAnterior));
        }

        public Transacao Aprovar()
        {
            _status.Aprovar();
            AddEvent(new TransacaoCartaoAprovadaEvent(Id.ToString(), CobrancaId));
            return this;
        }

        public Transacao DefinirErro(string mensagemErro)
        {
            _status.DefinirErro(mensagemErro);
            AddEvent(new ErroTransacaoCartaoEvent(Id.ToString()));
            return this;
        }

        public Transacao Reprocessar(decimal valor, string contaBancariaId) 
            => new Transacao(CobrancaId, contaBancariaId, valor, Status.Reprocessar(), this);
    }
}