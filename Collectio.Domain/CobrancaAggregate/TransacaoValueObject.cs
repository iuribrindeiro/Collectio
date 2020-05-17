using System;
using Collectio.Domain.CobrancaAggregate.Exceptions;

namespace Collectio.Domain.CobrancaAggregate
{
    public class TransacaoValueObject
    {
        private FormaPagamento _formaPagamento;
        private StatusTransacao _status;

        public FormaPagamento FormaPagamento => _formaPagamento;
        public StatusTransacao Status => _status;

        private TransacaoValueObject(FormaPagamento formaPagamento)
        {
            _formaPagamento = formaPagamento;
            _status = StatusTransacao.Processando;
        }

        public static TransacaoValueObject Cartao()
            => new TransacaoValueObject(FormaPagamento.Cartao);

        public static TransacaoValueObject Boleto()
            => new TransacaoValueObject(FormaPagamento.Boleto);

        public TransacaoValueObject AlterarFormaPagamento(FormaPagamento formaPagamento)
        {
            if (FormaPagamento != formaPagamento && ProcessamentoPendente)
                throw new TransacaoAindaEmProcessamentoException(FormaPagamento);

            return new TransacaoValueObject(formaPagamento);
        }

        public TransacaoValueObject Reprocessar() 
            => new TransacaoValueObject(FormaPagamento);

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

        public static bool operator ==(TransacaoValueObject a, TransacaoValueObject b)
            => a.FormaPagamento == b.FormaPagamento;

        public static bool operator !=(TransacaoValueObject a, TransacaoValueObject b)
            => a.FormaPagamento != b.FormaPagamento;
    }
}