using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.CobrancaAggregate.Exceptions;
using Collectio.Domain.CobrancaAggregate.Pagamentos;
using System;

namespace Collectio.Domain.CobrancaAggregate
{
    public class Cobranca : BaseTenantEntity, IAggregateRoot
    {
        private decimal _valor;
        private DateTime _vencimento;
        private Guid _pagadorId;
        private Pagador _pagador;
        private Pagamento _pagamento;
        private string _contaBancariaId;
        private TransacaoValueObject _transacao;

        public decimal Valor => _valor;
        public DateTime Vencimento => _vencimento;
        public StatusCobranca Status => Pagamento ? StatusCobranca.Pago : 
            Vencimento < DateTime.Today ? StatusCobranca.Vencido : 
            StatusCobranca.Pendente;
        public bool FormaPagamentoBoleto => _transacao.FormaPagamentoBoleto;
        public bool FormaPagamentoCartao => _transacao.FormaPagamentoCartao;
        public Pagamento Pagamento => _pagamento;
        public TransacaoValueObject Transacao => _transacao;
        public Guid PagadorId => _pagadorId;
        public virtual Pagador Pagador => _pagador;
        public string ContaBancariaId => _contaBancariaId;


        public static Cobranca Cartao(decimal valor, DateTime vencimento, string clienteId, string cartaoCreditoId, string contaBancariaId)
            => new Cobranca(valor, vencimento, clienteId, contaBancariaId, TransacaoValueObject.Cartao(), cartaoCreditoId);

        public static Cobranca Boleto(decimal valor, DateTime vencimento, string clienteId, string contaBancariaId)
            => new Cobranca(valor, vencimento, clienteId, contaBancariaId, TransacaoValueObject.Boleto());

        private Cobranca(
            decimal valor, DateTime vencimento, string clienteId, string contaBancariaId, TransacaoValueObject transacao, string cartaoCreditoId = null)
        {
            _valor = valor;
            _vencimento = vencimento;
            _pagador = new Pagador(clienteId, cartaoCreditoId);
            _contaBancariaId = contaBancariaId;
            _transacao = transacao;

            AddEvent(new CobrancaCriadaEvent(Id.ToString()));
        }

        public Cobranca AlterarCobranca(decimal valor, DateTime vencimento, string clienteId, string contaBancariaId, string cartaoCreditoId = null)
        {
            if (Transacao.ProcessamentoPendente)
                throw new ImpossivelAlterarCobrancaComFormaPagamentoPendenteException();

            if (Status == StatusCobranca.Pago)
                throw new ImpossivelAlterarCobrancaPagaException();

            var valorAnterior = _valor;
            var vencimentoAnterior = _vencimento;
            var clienteAnterior = _pagador.ClienteId;
            var cartaoCreditoIdAnterior = _pagador.CartaoCreditoId;
            var contaBancariaIdAnterior = _contaBancariaId;

            var valoresPagamentoMudaram = ValoresPagamentoMudaram(valor, clienteId, cartaoCreditoId, contaBancariaId, 
                valorAnterior, clienteAnterior, cartaoCreditoIdAnterior, contaBancariaIdAnterior);

            if (valoresPagamentoMudaram)
                ReprocessarTransacao();

            _valor = valor;
            _vencimento = vencimento;
            _pagador.Alterar(clienteId, cartaoCreditoId);
            _contaBancariaId = contaBancariaId;

            AddEvent(new CobrancaAlteradaEvent(valorAnterior, vencimentoAnterior, clienteAnterior, cartaoCreditoIdAnterior, contaBancariaIdAnterior, Id.ToString()));
            return this;
        }

        public Cobranca AlterarFormaPagamento(FormaPagamento formaPagamento)
        {
            var formaPagamentoAnterior = _transacao.FormaPagamento;
            _transacao = _transacao.AlterarFormaPagamento(formaPagamento);
            AddEvent(new FormaPagamentoAlteradaEvent(Id.ToString(), formaPagamentoAnterior));
            return this;
        }

        public Cobranca RealizarPagamento(decimal valor)
        {
            if (Transacao.ProcessamentoPendente || Transacao.Status == StatusTransacao.Erro)
                throw new FormaPagamentoNaoProcessadaException();

            _pagamento = new Pagamento(valor);
            AddEvent(new PagamentoRealizadoEvent(this));
            return this;
        }

        public Cobranca ReprocessarTransacao()
        {
            if (Status == StatusCobranca.Pago)
                throw new ImpossivelRegerarFormaPagamentoParaCobrancaPagaException();

            if (Transacao.ProcessamentoPendente)
                throw new ImpossivelRegerarFormaQuandoFormaPagamentoPendenteException();

            _transacao = _transacao.Reprocessar();
            AddEvent(new TransacaoCobrancaReprocessandodoEvent(Id.ToString()));
            return this;
        }

        public Cobranca FinalizaProcessamentoFormaPagamento()
        {
            _transacao.FinalizaProcessamento();
            if (_transacao.FormaPagamentoCartao)
                RealizarPagamento(Valor);

            AddEvent(new FormaPagamentoProcessadaEvent(this));
            return this;
        }

        public Cobranca ErroCriarTransacao()
        {
            _transacao.Erro();
            AddEvent(new FalhaAoProcessarFormaPagamentoEvent(this));
            return this;
        }

        private static bool ValoresPagamentoMudaram(
            decimal valor, string clienteId, string cartaoCreditoId,
            string contaBancariaId, decimal valorAnterior, string clienteIdAnterior, 
            string cartaoCreditoIdAnterior, string contaBancariaIdAnterior)
            => valor != valorAnterior || clienteId != clienteIdAnterior || cartaoCreditoId != cartaoCreditoIdAnterior || contaBancariaId != contaBancariaIdAnterior;
    }
}
