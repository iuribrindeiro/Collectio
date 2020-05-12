using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate.AjustesValorPagamento;
using Collectio.Domain.CobrancaAggregate.ContaBancarias;
using Collectio.Domain.CobrancaAggregate.Entidades;
using Collectio.Domain.CobrancaAggregate.Pagamentos;
using System;

namespace Collectio.Domain.CobrancaAggregate
{
    public enum StatusFormaPagamento
    {
        Criando,
        Criado,
        Erro
    }

    public class FormaPagamentoValueObject
    {
        private TipoFormaPagamento _tipo;
        private string _formaPagamentoId;
        private StatusFormaPagamento _status;

        public TipoFormaPagamento Tipo => _tipo;
        public string FormaPagamentoId => _formaPagamentoId;
        public StatusFormaPagamento Status => _status;

        private FormaPagamentoValueObject(TipoFormaPagamento tipo)
        {
            _tipo = tipo;
            _status = StatusFormaPagamento.Criando;
        }

        public void ConfirmarCriacao(string formaPagamentoId)
        {
            _formaPagamentoId = formaPagamentoId;
            _status = StatusFormaPagamento.Criado;
        }

        public void FalhaCriacao() 
            => _status = StatusFormaPagamento.Erro;

        public static FormaPagamentoValueObject Cartao() 
            => new FormaPagamentoValueObject(TipoFormaPagamento.Cartao);

        public static FormaPagamentoValueObject Boleto()
            => new FormaPagamentoValueObject(TipoFormaPagamento.Boleto);
    }

    public class Cobranca : BaseTenantEntity, IAggregateRoot
    {
        private decimal _valor;
        private DateTime _vencimento;
        private Pagador _pagador;
        private Emissor _emissor;
        private Pagamento _pagamento;
        private ContaBancaria _contaBancaria;
        private Juros _juros;
        private Desconto _desconto;
        private Multa _multa;
        private StatusCobranca _status;
        private FormaPagamentoValueObject _formaPagamento;

        public decimal Valor => _valor;
        public DateTime Vencimento => _vencimento;
        public Pagador Pagador => _pagador;
        public Emissor Emissor => _emissor;
        public Pagamento Pagamento => _pagamento;
        public ContaBancaria ContaBancaria => _contaBancaria;
        public Juros Juros => _juros;
        public Multa Multa => _multa;
        public Desconto Desconto => _desconto;
        public virtual StatusCobranca Status => _status;
        public FormaPagamentoValueObject FormaPagamento => _formaPagamento;

        public Cobranca(decimal valor, DateTime vencimento, Pagador pagador,
            Emissor emissor, ContaBancaria contaBancaria, FormaPagamentoValueObject formaPagamento,
            Juros juros = null, Multa multa = null, Desconto desconto = null)
        {
            _valor = valor;
            _vencimento = vencimento;
            _pagador = pagador;
            _emissor = emissor;
            _contaBancaria = contaBancaria;
            _formaPagamento = formaPagamento;
            _juros = juros;
            _multa = multa;
            _desconto = desconto;
        }
    }
}
