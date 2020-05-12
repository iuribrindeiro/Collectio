using System;
using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate.AjustesValorPagamento;
using Collectio.Domain.CobrancaAggregate.Entidades;
using Collectio.Domain.CobrancaAggregate.Pagamentos;
using Collectio.Domain.CobrancaAggregate.ContaBancarias;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate
{
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
        private FormaPagamento _formaPagamento;
        private string _formaPagamentoId;
        private StatusCobranca _status;

        public decimal Valor => _valor;
        public DateTime Vencimento => _vencimento;
        public Pagador Pagador => _pagador;
        public Emissor Emissor => _emissor;
        public Pagamento Pagamento => _pagamento;
        public ContaBancaria ContaBancaria => _contaBancaria;
        public Juros Juros => _juros;
        public Multa Multa => _multa;
        public Desconto Desconto => _desconto;
        public FormaPagamento FormaPagamento => _formaPagamento;
        public string FormaPagamentoId => _formaPagamentoId;
        public virtual StatusCobranca Status => _status;

        public Cobranca(decimal valor, DateTime vencimento, Pagador pagador,
            Emissor emissor, ContaBancaria contaBancaria, FormaPagamento formaPagamento,
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

        protected override IValidator ValidatorFactory() 
            => new CobrancaValidator();
    }
}
