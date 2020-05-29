using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.CobrancaAggregate.Exceptions;
using Collectio.Domain.CobrancaAggregate.Pagamentos;
using System;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Domain.CobrancaAggregate
{
    public class Cobranca : BaseOwnerEntity, IAggregateRoot
    {
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime Vencimento { get; private set; }
        public virtual StatusCobranca Status => Pagamento ? StatusCobranca.Pago :
            Vencimento < DateTime.Today ? StatusCobranca.Vencido : 
            StatusCobranca.Pendente;

        public virtual bool FormaPagamentoBoleto => Transacao.FormaPagamentoBoleto;
        public virtual bool FormaPagamentoCartao => Transacao.FormaPagamentoCartao;
        public Pagamento Pagamento { get; private set; }
        public Transacao Transacao { get; private set; }
        public virtual ClienteCobranca ClienteCobranca { get; private set; }
        public string ConfiguracaoEmissaoId { get; private set; }

        private Cobranca(){}

        public static Cobranca Cartao(
            string decricao, decimal valor, DateTime vencimento, string configuracaoEmissaoId, string nomeCliente, 
            string cpfCnpjCliente, string emailCliente, Telefone telefoneCliente, 
            CartaoCreditoCobranca cartaoCreditoCliente, Endereco enderecoCliente = null, string tenantIdCliente = null) 
            => new Cobranca(decricao, valor, vencimento, configuracaoEmissaoId, 
                Transacao.Cartao(), nomeCliente, cpfCnpjCliente, emailCliente, 
                telefoneCliente, enderecoCliente, tenantIdCliente, cartaoCreditoCliente);

        public static Cobranca Boleto(
            string decricao, decimal valor, DateTime vencimento, string configuracaoEmissaoId, 
            string nomeCliente, string cpfCnpjCliente, string emailCliente, Telefone telefoneCliente, 
            Endereco endercoCliente, string tenantIdCliente = null)
            => new Cobranca(decricao, valor, vencimento, configuracaoEmissaoId, Transacao.Boleto(), 
                nomeCliente, cpfCnpjCliente, emailCliente, telefoneCliente, endercoCliente, tenantIdCliente);

        private Cobranca(string decricao, decimal valor, DateTime vencimento, string configuracaoEmissaoId, 
            Transacao transacao, string nomeCliente, string cpfCnpjCliente, string emailCliente, Telefone telefoneCliente,
            Endereco enderecoCliente, string tenantIdCliente, CartaoCreditoCobranca cartaoCreditoCobranca = null)
        {
            Descricao = decricao;
            Valor = valor;
            Vencimento = vencimento;
            ConfiguracaoEmissaoId = configuracaoEmissaoId;
            Transacao = transacao;
            ClienteCobranca = new ClienteCobranca(this, nomeCliente, cpfCnpjCliente, emailCliente, tenantIdCliente, telefoneCliente, enderecoCliente, cartaoCreditoCobranca);
            AddEvent(new CobrancaCriadaEvent(Id.ToString()));
        }

        public Cobranca AlterarCobranca(decimal valor, DateTime vencimento, string configuracaoEmissaoId)
        {
            if (Transacao.ProcessamentoPendente)
                throw new ImpossivelAlterarCobrancaComFormaPagamentoPendenteException();

            if (Status == StatusCobranca.Pago)
                throw new ImpossivelAlterarCobrancaPagaException();

            var valorAnterior = Valor;
            var vencimentoAnterior = Vencimento;
            var configuracaoEmissaoIdAnterior = ConfiguracaoEmissaoId;

            Valor = valor;
            Vencimento = vencimento;
            ConfiguracaoEmissaoId = configuracaoEmissaoId;

            if (FormaPagamentoBoleto)
                ReprocessarTransacao();

            AddEvent(new CobrancaAlteradaEvent(valorAnterior, vencimentoAnterior, configuracaoEmissaoIdAnterior, Id.ToString()));
            return this;
        }

        public Cobranca AlterarFormaPagamento(FormaPagamento formaPagamento)
        {
            var formaPagamentoAnterior = Transacao.FormaPagamento;
            Transacao = Transacao.AlterarFormaPagamento(formaPagamento);
            AddEvent(new FormaPagamentoAlteradaEvent(Id.ToString(), formaPagamentoAnterior));
            return this;
        }

        public Cobranca RealizarPagamento(decimal valor)
        {
            if (Transacao.ProcessamentoPendente || Transacao.Status == StatusTransacao.Erro)
                throw new FormaPagamentoNaoProcessadaException();

            Pagamento = new Pagamento(valor);
            AddEvent(new PagamentoRealizadoEvent(this));
            return this;
        }

        public Cobranca ReprocessarTransacao()
        {
            if (Status == StatusCobranca.Pago)
                throw new ImpossivelRegerarFormaPagamentoParaCobrancaPagaException();

            if (Transacao.ProcessamentoPendente)
                throw new ImpossivelRegerarFormaQuandoFormaPagamentoPendenteException();

            Transacao = Transacao.Reprocessar();
            AddEvent(new TransacaoCobrancaReprocessandodoEvent(Id.ToString()));
            return this;
        }

        public Cobranca FinalizaProcessamentoFormaPagamento()
        {
            Transacao.FinalizaProcessamento();
            if (Transacao.FormaPagamentoCartao)
                RealizarPagamento(Valor);

            AddEvent(new FormaPagamentoProcessadaEvent(this));
            return this;
        }

        public Cobranca ErroCriarTransacao()
        {
            Transacao.Erro();
            AddEvent(new FalhaAoProcessarFormaPagamentoEvent(this));
            return this;
        }
    }
}
