using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate.AjustesValorPagamento;
using Collectio.Domain.CobrancaAggregate.Pagamentos;
using System;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.CobrancaAggregate.Exceptions;

namespace Collectio.Domain.CobrancaAggregate
{
    public class Cobranca : BaseTenantEntity, IAggregateRoot
    {
        private decimal _valor;
        private DateTime _vencimento;
        private string _pagadorId;
        private string _emissorId;
        private Pagamento _pagamento;
        private string _contaBancariaId;
        private JurosValueObject _juros;
        private DescontoValueObject _desconto;
        private MultaValueObject _multa;
        private StatusCobranca _status;
        private FormaPagamentoValueObject _formaPagamento;

        public decimal Valor => _valor;
        public DateTime Vencimento => _vencimento;
        public virtual StatusCobranca Status => _status;
        public Pagamento Pagamento => _pagamento;
        public JurosValueObject Juros => _juros;
        public MultaValueObject Multa => _multa;
        public DescontoValueObject Desconto => _desconto;
        public FormaPagamentoValueObject FormaPagamento => _formaPagamento;
        public string PagadorId => _pagadorId;
        public string EmissorId => _emissorId;
        public string ContaBancariaId => _contaBancariaId;


        public static Cobranca Cartao(decimal valor, DateTime vencimento, string pagadorId,
            string emissorId, string contaBancariaId, JurosValueObject juros = null, MultaValueObject multa = null, DescontoValueObject desconto = null) 
            => new Cobranca(valor, vencimento, pagadorId, emissorId, contaBancariaId, FormaPagamentoValueObject.Cartao(), juros, multa, desconto);

        public static Cobranca Boleto(decimal valor, DateTime vencimento, string pagadorId,
            string emissorId, string contaBancariaId, JurosValueObject juros = null, MultaValueObject multa = null, DescontoValueObject desconto = null)
            => new Cobranca(valor, vencimento, pagadorId, emissorId, contaBancariaId, FormaPagamentoValueObject.Boleto(), juros, multa, desconto);

        private Cobranca(decimal valor, DateTime vencimento, string pagadorId,
            string emissorId, string contaBancariaId, FormaPagamentoValueObject formaPagamento,
            JurosValueObject juros = null, MultaValueObject multa = null, DescontoValueObject desconto = null)
        {
            _valor = valor;
            _vencimento = vencimento;
            _pagadorId = pagadorId;
            _emissorId = emissorId;
            _contaBancariaId = contaBancariaId;
            _formaPagamento = formaPagamento;
            _juros = juros;
            _multa = multa;
            _desconto = desconto;

            AddEvent(new CobrancaCriadaEvent(this));
        }

        public void AlterarFormaPagamento(FormaPagamentoValueObject formaPagamento)
        {
            if (FormaPagamento != formaPagamento && FormaPagamento.ProcessamentoPendente)
                throw new FormaPagamentoAindaEmProcessamentoException(formaPagamento);

            var formaPagamentoAnterior = _formaPagamento;

            _formaPagamento = formaPagamento;
            AddEvent(new FormaPagamentoAlteradaEvent(this, formaPagamentoAnterior));
        }

        public void IniciarProcessamentoFormaPagamento() 
            => _formaPagamento.IniciarProcessamento();

        public void FinalizaProcessamentoFormaPagamento(string id)
            => _formaPagamento.FinalizaProcessamento(id);

        public class FormaPagamentoValueObject
        {
            private string _formaPagamentoId;
            private TipoFormaPagamento _tipo;
            private StatusFormaPagamento _status;

            public string FormaPagamentoId => _formaPagamentoId;
            public TipoFormaPagamento Tipo => _tipo;
            public StatusFormaPagamento Status => _status;

            private FormaPagamentoValueObject(TipoFormaPagamento tipo)
            {
                _tipo = tipo;
                _status = StatusFormaPagamento.AguardandoInicioProcessamento;
            }

            public static FormaPagamentoValueObject Cartao()
                => new FormaPagamentoValueObject(TipoFormaPagamento.Cartao);

            public static FormaPagamentoValueObject Boleto()
                => new FormaPagamentoValueObject(TipoFormaPagamento.Boleto);

            public bool ProcessamentoPendente 
                => !ProcessamentoConcluido;

            public bool ProcessamentoConcluido =>
                _status == StatusFormaPagamento.Criado || _status == StatusFormaPagamento.Erro;

            internal void IniciarProcessamento()
                => _status = StatusFormaPagamento.Processando;

            internal void FinalizaProcessamento(string id)
            {
                _formaPagamentoId = id;
                _status = StatusFormaPagamento.Criado;
            }

            public static bool operator ==(FormaPagamentoValueObject a, FormaPagamentoValueObject b) 
                => !(a != b);

            public static bool operator !=(FormaPagamentoValueObject a, FormaPagamentoValueObject b)
                => ((a?.FormaPagamentoId == null && a?.FormaPagamentoId == null) ||
                    (a?.FormaPagamentoId != b?.FormaPagamentoId)) && (a?.Tipo != b?.Tipo);
        }
    }
}
