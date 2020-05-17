using System;
using Collectio.Domain.CobrancaAggregate.Exceptions;

namespace Collectio.Domain.CobrancaAggregate
{
    public class FormaPagamentoValueObject
    {
        private string _id;
        private TipoFormaPagamento _tipo;
        private StatusFormaPagamento _status;
        private string _idAnterior;

        public TipoFormaPagamento Tipo => _tipo;
        public StatusFormaPagamento Status => _status;
        public string Id => _id;
        public string IdAnterior => _idAnterior;

        private FormaPagamentoValueObject(TipoFormaPagamento tipo, string id)
        {
            _tipo = tipo;
            _status = StatusFormaPagamento.Processando;
            _id = id;
        }

        private FormaPagamentoValueObject(TipoFormaPagamento tipo, string id, string idAnterior)
        {
            _tipo = tipo;
            _status = StatusFormaPagamento.Processando;
            _id = id;
            _idAnterior = idAnterior;
        }

        public static FormaPagamentoValueObject Cartao(string cartaoId)
            => new FormaPagamentoValueObject(TipoFormaPagamento.Cartao, cartaoId);

        public static FormaPagamentoValueObject Boleto()
            => new FormaPagamentoValueObject(TipoFormaPagamento.Boleto, Guid.NewGuid().ToString());

        public static FormaPagamentoValueObject Boleto(string idAnterior)
            => new FormaPagamentoValueObject(TipoFormaPagamento.Boleto, Guid.NewGuid().ToString(), idAnterior);

        public FormaPagamentoValueObject Regerar()
        {
            if (FormaPagamentoCartao)
                return new FormaPagamentoValueObject(TipoFormaPagamento.Cartao, Id, Id);

            return new FormaPagamentoValueObject(TipoFormaPagamento.Boleto, Guid.NewGuid().ToString(), Id);
        }

        public FormaPagamentoValueObject RegerarCartao()
            => new FormaPagamentoValueObject(TipoFormaPagamento.Cartao, Id, Id);

        public bool FormaPagamentoBoleto
            => Tipo == TipoFormaPagamento.Boleto;

        public bool FormaPagamentoCartao
            => Tipo == TipoFormaPagamento.Cartao;

        public bool ProcessamentoPendente
            => !ProcessamentoConcluido;

        public bool ProcessamentoConcluido =>
            _status == StatusFormaPagamento.Criado || _status == StatusFormaPagamento.Erro;

        public void FinalizaProcessamento()
        {
            if (Status == StatusFormaPagamento.Criado || Status == StatusFormaPagamento.Erro)
                throw new ProcessoFormaPagamentoJaFinalizadoException();

            _status = StatusFormaPagamento.Criado;
        }

        public void ErroCriarFormaPagamento()
        {
            if (Status == StatusFormaPagamento.Criado || Status == StatusFormaPagamento.Erro)
                throw new ProcessoFormaPagamentoJaFinalizadoException();

            _status = StatusFormaPagamento.Erro;
        }

        public static bool operator ==(FormaPagamentoValueObject a, FormaPagamentoValueObject b)
            => a.Id == b.Id;

        public static bool operator !=(FormaPagamentoValueObject a, FormaPagamentoValueObject b)
            => a.Id != b.Id;
    }
}