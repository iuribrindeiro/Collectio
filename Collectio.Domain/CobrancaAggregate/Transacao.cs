using System;
using Collectio.Domain.CobrancaAggregate.Exceptions;

namespace Collectio.Domain.CobrancaAggregate
{
    public class Transacao
    {
        private FormaPagamento _formaPagamento;
        private StatusTransacao _status;

        public FormaPagamento FormaPagamento => _formaPagamento;
        public StatusTransacao Status => _status;

        private Transacao(FormaPagamento formaPagamento)
        {
            _formaPagamento = formaPagamento;
            _status = StatusTransacao.Processando;
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

        public bool FormaPagamentoBoleto
            => FormaPagamento == FormaPagamento.Boleto;

        public bool FormaPagamentoCartao
            => FormaPagamento == FormaPagamento.Cartao;

        public bool ProcessamentoPendente
            => !ProcessamentoConcluido;

        public bool ProcessamentoConcluido =>
            _status == StatusTransacao.Processado || _status == StatusTransacao.Erro;

        public void FinalizaProcessamento()
        {
            if (Status == StatusTransacao.Processado || Status == StatusTransacao.Erro)
                throw new ProcessoFormaPagamentoJaFinalizadoException();

            _status = StatusTransacao.Processado;
        }

        public void Erro()
        {
            if (Status == StatusTransacao.Processado || Status == StatusTransacao.Erro)
                throw new ProcessoFormaPagamentoJaFinalizadoException();

            _status = StatusTransacao.Erro;
        }

        public static bool operator ==(Transacao a, Transacao b)
            => a.FormaPagamento == b.FormaPagamento;

        public static bool operator !=(Transacao a, Transacao b)
            => a.FormaPagamento != b.FormaPagamento;
    }
}