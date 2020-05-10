using System;
using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate.AjustesValorPagamento;
using Collectio.Domain.CobrancaAggregate.Entidades;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate
{
    public class Cobranca : BaseTenantEntity, IAggregateRoot
    {
        private decimal _valor;
        private DateTime _vencimento;
        private Pagador _pagador;
        private Emissor _emissor;
        private Pagamento.Pagamento _pagamento;
        private ContaBancaria.ContaBancaria _contaBancaria;
        private Juros _juros;
        private Desconto _desconto;
        private Multa _multa;
        private FormaPagamentoCobranca _formaPagamento;

        public decimal Valor => _valor;
        public DateTime Vencimento => _vencimento;
        public Pagador Pagador => _pagador;
        public Emissor Emissor => _emissor;
        public Pagamento.Pagamento Pagamento => _pagamento;
        public ContaBancaria.ContaBancaria ContaBancaria => _contaBancaria;
        public Juros Juros => _juros;
        public Multa Multa => _multa;
        public Desconto Desconto => _desconto;
        public FormaPagamentoCobranca FormaPagamento => _formaPagamento;

        public virtual StatusCobranca Status
        {
            get
            {
                if (!Pagamento)
                    return DateTime.Today > Vencimento ? StatusCobranca.Vencido : StatusCobranca.Pendente;

                return StatusCobranca.Pago;
            }
        }

        public Cobranca(decimal valor, DateTime vencimento, Pagador pagador,
            Emissor emissor, ContaBancaria.ContaBancaria contaBancaria, FormaPagamentoCobranca formaPagamento,
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
