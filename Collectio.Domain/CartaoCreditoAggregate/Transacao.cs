using Collectio.Domain.Base;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using Collectio.Domain.ClienteAggregate.Exceptions;
using System;

namespace Collectio.Domain.CartaoCreditoAggregate
{
    public class Transacao : BaseOwnerEntity
    {
        public string CobrancaId { get; private set; }

        public decimal Valor { get; private set; }

        public Guid CartaoId { get; private set; }

        public virtual CartaoCredito CartaoCredito { get; private set; }

        public StatusTransacaoCartaoValueObject Status { get; private set; }


        private Transacao() {}

        public Transacao(string cobrancaId, CartaoCredito cartaoCredito, decimal valor)
        {
            if (!cartaoCredito.ProcessamentoFinalizado)
                throw new CartaoCreditoNaoProcessadoException();

            CobrancaId = cobrancaId;
            CartaoId = cartaoCredito.Id;
            Valor = valor;
            Status = StatusTransacaoCartaoValueObject.Processando();
            AddEvent(new TransacaoCartaoCriadaEvent(Id.ToString()));
        }

        private Transacao(string cobrancaId, decimal valor, StatusTransacaoCartaoValueObject statusTransacao, Transacao transacaoAnterior)
        {
            CobrancaId = cobrancaId;
            Valor = valor;
            Status = statusTransacao;
            AddEvent(new ReprocessandoTransacaoCartaoEvent(this, transacaoAnterior));
        }

        public Transacao Aprovar()
        {
            Status.Aprovar();
            AddEvent(new TransacaoCartaoAprovadaEvent(Id.ToString(), CobrancaId));
            return this;
        }

        public Transacao DefinirErro(string mensagemErro)
        {
            Status.DefinirErro(mensagemErro);
            AddEvent(new ErroTransacaoCartaoEvent(Id.ToString()));
            return this;
        }

        public Transacao Reprocessar(decimal valor) 
            => new Transacao(CobrancaId, valor, Status.Reprocessar(), this);
    }
}