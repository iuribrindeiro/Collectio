using System;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CobrancaAggregate.Exceptions;

namespace Collectio.Domain.CobrancaAggregate
{
    public class Transacao : ValueObject
    {
        public FormaPagamento FormaPagamento { get; private set; }

        public StatusTransacao Status { get; private set; }

        private Transacao() {}

        private Transacao(FormaPagamento formaPagamento)
        {
            FormaPagamento = formaPagamento;
            Status = StatusTransacao.Processando;
        }

        public static Transacao Cartao()
            => new Transacao(FormaPagamento.Cartao);

        public static Transacao Boleto()
            => new Transacao(FormaPagamento.Boleto);

        public Transacao AlterarFormaPagamento(FormaPagamento formaPagamento)
        {
            if (FormaPagamento != formaPagamento && ProcessamentoPendente)
                throw new TransacaoAindaEmProcessamentoException(FormaPagamento);

            return new Transacao(formaPagamento);
        }

        public Transacao Reprocessar() 
            => new Transacao(FormaPagamento);

        public virtual bool FormaPagamentoBoleto
            => FormaPagamento == FormaPagamento.Boleto;

        public virtual bool FormaPagamentoCartao
            => FormaPagamento == FormaPagamento.Cartao;

        public virtual bool ProcessamentoPendente
            => !ProcessamentoConcluido;

        public virtual bool ProcessamentoConcluido =>
            Status == StatusTransacao.Processado || Status == StatusTransacao.Erro;

        public void FinalizaProcessamento()
        {
            if (Status == StatusTransacao.Processado || Status == StatusTransacao.Erro)
                throw new ProcessoFormaPagamentoJaFinalizadoException();

            Status = StatusTransacao.Processado;
        }

        public void Erro()
        {
            if (Status == StatusTransacao.Processado || Status == StatusTransacao.Erro)
                throw new ProcessoFormaPagamentoJaFinalizadoException();

            Status = StatusTransacao.Erro;
        }

        public static bool operator ==(Transacao a, Transacao b)
            => a.FormaPagamento == b.FormaPagamento;

        public static bool operator !=(Transacao a, Transacao b)
            => a.FormaPagamento != b.FormaPagamento;
    }
}