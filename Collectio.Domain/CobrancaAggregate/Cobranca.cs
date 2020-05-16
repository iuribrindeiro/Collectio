using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.CobrancaAggregate.Exceptions;
using Collectio.Domain.CobrancaAggregate.Pagamentos;
using System;

namespace Collectio.Domain.CobrancaAggregate
{
    public sealed class Cobranca : BaseTenantEntity, IAggregateRoot
    {
        private decimal _valor;
        private DateTime _vencimento;
        private string _pagadorId;
        private string _emissorId;
        private Pagamento _pagamento;
        private string _contaBancariaId;
        private FormaPagamentoValueObject _formaPagamento;

        public decimal Valor => _valor;
        public DateTime Vencimento => _vencimento;
        public StatusCobranca Status => Pagamento ? StatusCobranca.Pago : 
            Vencimento < DateTime.Today ? StatusCobranca.Vencido : 
            StatusCobranca.Pendente;

        public bool FormaPagamentoBoleto => _formaPagamento.FormaPagamentoBoleto;
        public bool FormaPagamentoCartao => _formaPagamento.FormaPagamentoCartao;
        public Pagamento Pagamento => _pagamento;
        public FormaPagamentoValueObject FormaPagamento => _formaPagamento;
        public string PagadorId => _pagadorId;
        public string EmissorId => _emissorId;
        public string ContaBancariaId => _contaBancariaId;


        public static Cobranca Cartao(decimal valor, DateTime vencimento, string pagadorId, string emissorId, string contaBancariaId)
            => new Cobranca(valor, vencimento, pagadorId, emissorId, contaBancariaId, FormaPagamentoValueObject.Cartao());

        public static Cobranca Boleto(decimal valor, DateTime vencimento, string pagadorId,  string emissorId, string contaBancariaId)
            => new Cobranca(valor, vencimento, pagadorId, emissorId, contaBancariaId, FormaPagamentoValueObject.Boleto());

        private Cobranca(decimal valor, DateTime vencimento, string pagadorId,
            string emissorId, string contaBancariaId, FormaPagamentoValueObject formaPagamento)
        {
            _valor = valor;
            _vencimento = vencimento;
            _pagadorId = pagadorId;
            _emissorId = emissorId;
            _contaBancariaId = contaBancariaId;
            _formaPagamento = formaPagamento;

            AddEvent(new CobrancaCriadaEvent(this));
        }

        public Cobranca AlterarCobranca(decimal valor, DateTime vencimento, string emissorId, string pagadorId, string contaBancariaId)
        {
            if (FormaPagamento.ProcessamentoPendente)
                throw new ImpossivelAlterarCobrancaComFormaPagamentoPendenteException();

            if (Status == StatusCobranca.Pago)
                throw new ImpossivelAlterarCobrancaPagaException();

            var valorAnterior = _valor;
            var vencimentoAnterior = _vencimento;
            var emissorIdAnterior = _emissorId;
            var pagadorIdAnterior = _pagadorId;
            var contaBancariaIdAnterior = _contaBancariaId;

            var valoresPagamentoMudaram = ValoresPagamentoMudaram(valor, emissorId, pagadorId, contaBancariaId, 
                valorAnterior, emissorIdAnterior, pagadorIdAnterior, contaBancariaIdAnterior);

            if (valoresPagamentoMudaram) 
                RegerarFormaPagamento();

            _valor = valor;
            _vencimento = vencimento;
            _emissorId = emissorId;
            _pagadorId = pagadorId;
            _contaBancariaId = contaBancariaId;

            AddEvent(new CobrancaAlteradaEvent(valorAnterior, vencimentoAnterior, pagadorIdAnterior, emissorIdAnterior, contaBancariaIdAnterior, this));
            return this;
        }

        private static bool ValoresPagamentoMudaram(
            decimal valor, string emissorId, string pagadorId, 
            string contaBancariaId, decimal valorAnterior, string emissorIdAnterior, 
            string pagadorIdAnterior, string contaBancariaIdAnterior) 
            => valor != valorAnterior || emissorId != emissorIdAnterior ||
            pagadorId != pagadorIdAnterior || contaBancariaId != contaBancariaIdAnterior;

        public Cobranca AlterarFormaPagamento(FormaPagamentoValueObject formaPagamento)
        {
            if (FormaPagamento != formaPagamento && FormaPagamento.ProcessamentoPendente)
                throw new FormaPagamentoAindaEmProcessamentoException(formaPagamento);

            var formaPagamentoAnterior = _formaPagamento;

            _formaPagamento = formaPagamento;
            AddEvent(new FormaPagamentoAlteradaEvent(this, formaPagamentoAnterior));

            return this;
        }

        public Cobranca RealizarPagamento(decimal valor)
        {
            if (FormaPagamento.ProcessamentoPendente || FormaPagamento.Status == StatusFormaPagamento.Erro)
                throw new FormaPagamentoNaoProcessadaException();

            _pagamento = new Pagamento(valor);
            AddEvent(new PagamentoRealizadoEvent(this));
            return this;
        }

        public Cobranca RegerarFormaPagamento()
        {
            if (Status == StatusCobranca.Pago)
                throw new ImpossivelRegerarFormaPagamentoParaCobrancaPagaException();

            if (FormaPagamento.ProcessamentoPendente)
                throw new ImpossivelRegerarFormaQuandoFormaPagamentoPendenteException();

            var formaPagamentoAnterior = _formaPagamento;

            _formaPagamento = _formaPagamento.Tipo == TipoFormaPagamento.Cartao
                ? FormaPagamentoValueObject.Cartao()
                : FormaPagamentoValueObject.Boleto();

            AddEvent(new FormaPagamentoRegeradaEvent(formaPagamentoAnterior, FormaPagamento));
            return this;
        }

        public Cobranca FinalizaProcessamentoFormaPagamento(string id)
        {
            _formaPagamento.FinalizaProcessamento(id);
            AddEvent(new FormaPagamentoProcessadaEvent(this));
            return this;
        }

        public Cobranca ErroCriarFormaPagamento()
        {
            _formaPagamento.ErroCriarFormaPagamento();
            AddEvent(new FalhaAoProcessarFormaPagamentoEvent(this));
            return this;
        }


        public class FormaPagamentoValueObject
        {
            private string _id;
            private TipoFormaPagamento _tipo;
            private StatusFormaPagamento _status;

            public TipoFormaPagamento Tipo => _tipo;
            public StatusFormaPagamento Status => _status;
            public string Id => _id;

            private FormaPagamentoValueObject(TipoFormaPagamento tipo)
            {
                _tipo = tipo;
                _status = StatusFormaPagamento.Processando;
            }

            public static FormaPagamentoValueObject Cartao()
                => new FormaPagamentoValueObject(TipoFormaPagamento.Cartao);

            public static FormaPagamentoValueObject Boleto()
                => new FormaPagamentoValueObject(TipoFormaPagamento.Boleto);

            public bool FormaPagamentoBoleto 
                => Tipo == TipoFormaPagamento.Boleto;

            public bool FormaPagamentoCartao
                => Tipo == TipoFormaPagamento.Cartao;

            public bool ProcessamentoPendente
                => !ProcessamentoConcluido;

            public bool ProcessamentoConcluido =>
                _status == StatusFormaPagamento.Criado || _status == StatusFormaPagamento.Erro;

            internal void FinalizaProcessamento(string id)
            {
                if (Status == StatusFormaPagamento.Criado || Status == StatusFormaPagamento.Erro)
                    throw new ProcessoFormaPagamentoJaFinalizadoException();

                _status = StatusFormaPagamento.Criado;
                _id = id;
            }

            internal void ErroCriarFormaPagamento()
            {
                if (Status == StatusFormaPagamento.Criado || Status == StatusFormaPagamento.Erro)
                    throw new ProcessoFormaPagamentoJaFinalizadoException();

                _status = StatusFormaPagamento.Erro;
            }

            public static bool operator ==(FormaPagamentoValueObject a, FormaPagamentoValueObject b)
                => a.Tipo == b.Tipo;

            public static bool operator !=(FormaPagamentoValueObject a, FormaPagamentoValueObject b)
                => a.Tipo != b.Tipo;
        }
    }
}
