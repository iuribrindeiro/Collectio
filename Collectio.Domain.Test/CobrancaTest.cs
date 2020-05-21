using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.CobrancaAggregate.Exceptions;
using NUnit.Framework;
using System;
using System.Linq;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Domain.Test
{
    public class CobrancaTest
    {
        private Cobranca _cobrancaBoleto;
        private Cobranca _cobrancaCartao;
        private Cobranca _cobrancaBoletoFormaPagamentoFinalizada;

        [SetUp]
        public void Setup()
        {
            _cobrancaBoleto = CobrancaBuilder.BuildCobrancaBoleto();
            _cobrancaCartao = CobrancaBuilder.BuildCobrancaCartao();
            _cobrancaBoletoFormaPagamentoFinalizada = CobrancaBuilder
                .BuildCobrancaBoleto()
                .ComTransacaoFinalizada();
        }

        [Test]
        public void AoCriarCobrancaCartaoOsValoresDevemSerSetadosCorretamente()
        {
            var valor = 20;
            var vencimento = DateTime.Today.AddDays(10);
            var tenantIdCliente = Guid.NewGuid().ToString();
            var cartaoCreditoId = Guid.NewGuid().ToString();
            var configuracaoEmissaoId = Guid.NewGuid().ToString();
            var nomeCliente = "Nome cliente teste 123";
            var cpfCnpjCliente = "123457822333";
            var emailCliente = "asdadad22233@email.com";
            var telefoneCliente = new Telefone("ddd", "4477885522");
            var cartaoCreditoCliente = new CartaoCredito("Nome clçiente 12313213", "numero 4455888", cartaoCreditoId);

            var novaCobranca = CobrancaBuilder.BuildCobrancaCartao(valor, vencimento, configuracaoEmissaoId, nomeCliente, cpfCnpjCliente, emailCliente, telefoneCliente, cartaoCreditoCliente, tenantIdCliente);

            Assert.AreEqual(novaCobranca.Valor, valor);
            Assert.AreEqual(novaCobranca.Vencimento, vencimento);
            Assert.AreEqual(novaCobranca.Cliente.TenantId, tenantIdCliente);
            Assert.AreEqual(novaCobranca.Cliente.CartaoCredito.TenantId, cartaoCreditoId);
            Assert.AreEqual(novaCobranca.Cliente.Telefone.Ddd, telefoneCliente.Ddd);
            Assert.AreEqual(novaCobranca.Cliente.Telefone.Numero, telefoneCliente.Numero);
            Assert.AreEqual(novaCobranca.Cliente.CartaoCredito.Nome, cartaoCreditoCliente.Nome);
            Assert.AreEqual(novaCobranca.Cliente.CartaoCredito.Numero, cartaoCreditoCliente.Numero);
            Assert.AreEqual(novaCobranca.Cliente.CartaoCredito.TenantId, cartaoCreditoCliente.TenantId);
            Assert.AreEqual(novaCobranca.ConfiguracaoEmissaoId, configuracaoEmissaoId);
        }

        [Test]
        public void ConsigoCriarCobrancaDeBoleto()
        {
            Assert.DoesNotThrow(() => CobrancaBuilder.BuildCobrancaBoleto());
        }

        [Test]
        public void NaoConsigoAlterarAFormaDePagamentoCasoAindaEstejaProcessando()
        {
            Assert.Throws<TransacaoAindaEmProcessamentoException>(() => _cobrancaBoleto.AlterarFormaPagamento(FormaPagamento.Cartao));
            Assert.Throws<TransacaoAindaEmProcessamentoException>(() => _cobrancaCartao.AlterarFormaPagamento(FormaPagamento.Boleto));
        }

        [Test]
        public void ConsigoAlterarAFormaDePagamentoCasoFormaPagamentAtualNaoEstejaProcessando()
        {
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento();
            _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();

            Assert.DoesNotThrow(() => _cobrancaBoleto.AlterarFormaPagamento(FormaPagamento.Cartao));
            Assert.DoesNotThrow(() => _cobrancaCartao.AlterarFormaPagamento(FormaPagamento.Boleto));
        }

        [Test]
        public void AoAlterarFormaPagamentoDeveCriarEventoNaCobranca()
        {
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento();
            _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();

            Assert.AreEqual(_cobrancaCartao.Events.Count(e => e is FormaPagamentoAlteradaEvent), 0);
            Assert.AreEqual(_cobrancaBoleto.Events.Count(e => e is FormaPagamentoAlteradaEvent), 0);

            var formaPagamentoCartaoAnterior = _cobrancaCartao.Transacao.FormaPagamento;
            var formaPagamentoBoletoAnterior = _cobrancaBoleto.Transacao.FormaPagamento;

            _cobrancaBoleto.AlterarFormaPagamento(FormaPagamento.Cartao);
            _cobrancaCartao.AlterarFormaPagamento(FormaPagamento.Boleto);

            var cobrancaCartaoEvent = _cobrancaCartao.Events
                .Where(e => e is FormaPagamentoAlteradaEvent)
                .Cast<FormaPagamentoAlteradaEvent>()
                .SingleOrDefault();
            var cobrancaBoletoEvent = _cobrancaBoleto.Events
                .Where(e => e is FormaPagamentoAlteradaEvent)
                .Cast<FormaPagamentoAlteradaEvent>()
                .SingleOrDefault();


            Assert.AreEqual(cobrancaBoletoEvent?.FormaPagamentoAnterior, formaPagamentoBoletoAnterior);
            Assert.AreEqual(cobrancaCartaoEvent?.FormaPagamentoAnterior, formaPagamentoCartaoAnterior);
            Assert.AreEqual(cobrancaBoletoEvent?.CobrancaId, _cobrancaBoleto.Id.ToString());
            Assert.AreEqual(cobrancaCartaoEvent?.CobrancaId, _cobrancaCartao.Id.ToString());
        }

        [Test]
        public void AoCriarUmaCobrancaDeveCriarEventoNaCobranca()
        {
            var novaCobranca = CobrancaBuilder.BuildCobrancaCartao();

            var cobrancaEvent = novaCobranca.Events
                .Where(e => e is CobrancaCriadaEvent)
                .Cast<CobrancaCriadaEvent>().SingleOrDefault();

            Assert.AreEqual(cobrancaEvent.CobrancaId, novaCobranca.Id.ToString());
        }

        [Test]
        public void StatusProcessamentoDeveSerIgualDeAcordoComSolicitado([Values(StatusTransacao.Processando, 
                StatusTransacao.Processado, StatusTransacao.Erro)] StatusTransacao statusTransacao)
        {
            var cobranca = _cobrancaBoleto;

            if (statusTransacao == StatusTransacao.Processando)
            {
                cobranca = CobrancaBuilder.BuildCobrancaCartao();
            } else if (statusTransacao == StatusTransacao.Processado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();
            } else if (statusTransacao == StatusTransacao.Erro)
            {
                _cobrancaBoleto.ErroCriarTransacao();
            }

            Assert.AreEqual(cobranca.Transacao.Status, statusTransacao);
        }

        [Test]
        public void AoTentarMudarStatusFormaPagamentoQuandoStatusNaoForPermitidoDeveLancarExcecao([Values(StatusTransacao.Processando,
            StatusTransacao.Processado, StatusTransacao.Erro)] StatusTransacao statusTransacao)
        {
            if (statusTransacao == StatusTransacao.Processado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento());
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.ErroCriarTransacao());
            } else if (statusTransacao == StatusTransacao.Erro)
            {
                _cobrancaBoleto.ErroCriarTransacao();
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento());
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.ErroCriarTransacao());
            }
        }

        [Test]
        public void AoMudarStatusFormaPagamentoDeveAdicionarEventoNaCobranca([Values(StatusTransacao.Processado, StatusTransacao.Erro)] StatusTransacao statusTransacao)
        {
            if (statusTransacao == StatusTransacao.Processado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();

                Assert.AreEqual(_cobrancaBoleto.Events
                    .Where(e => e is FormaPagamentoProcessadaEvent)
                    .Cast<FormaPagamentoProcessadaEvent>()
                    .SingleOrDefault()?.Cobranca, _cobrancaBoleto);
            }
            else if (statusTransacao == StatusTransacao.Erro)
            {
                _cobrancaBoleto.ErroCriarTransacao();

                Assert.AreEqual(_cobrancaBoleto.Events
                    .Where(e => e is FalhaAoProcessarFormaPagamentoEvent)
                    .Cast<FalhaAoProcessarFormaPagamentoEvent>()
                    .SingleOrDefault()?.Cobranca, _cobrancaBoleto);
            }
        }

        [Test]
        public void AoAdicionarPagamentoDeveAdicionarEventoNaCobranca()
        {
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(200);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Events
                .Where(e => e is PagamentoRealizadoEvent)
                .Cast<PagamentoRealizadoEvent>()
                .SingleOrDefault()?.Cobranca, _cobrancaBoletoFormaPagamentoFinalizada);
        }

        [Test]
        public void ConsigoFinalizarUmProcessamentoFormaPagamento()
        {
            Assert.DoesNotThrow(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento());
        }

        [Test]
        public void ConsigoDefinirComoErroUmProcessamentoFormaPagamento()
        {
            Assert.DoesNotThrow(() => _cobrancaBoleto.ErroCriarTransacao());
        }

        [Test]
        public void ConsigoRealizarPagamentoCobranca()
        {
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(200);
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(200);
        }

        [Test]
        public void DeveLancarExcecaoAoTentarRealizarPagamentoComFormaPagamentoNaoFinalizada()
        {
            Assert.Throws<FormaPagamentoNaoProcessadaException>(() => _cobrancaCartao.RealizarPagamento(200));
            _cobrancaCartao.ErroCriarTransacao();
            Assert.Throws<FormaPagamentoNaoProcessadaException>(() => _cobrancaCartao.RealizarPagamento(200));
        }

        [Test]
        public void AoAlterarCobrancaOsNovosValoresDevemEstarSetadosCorretamente()
        {
            var vencimento = DateTime.Today;
            var configuracaoEmissaoId = Guid.NewGuid().ToString();
            var clienteId = Guid.NewGuid().ToString();
            var valor = 20;

            _cobrancaBoletoFormaPagamentoFinalizada.AlterarCobranca(valor, vencimento, configuracaoEmissaoId);

            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Valor, valor);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Vencimento, vencimento);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.ConfiguracaoEmissaoId, configuracaoEmissaoId);
        }

        [Test]
        public void AoTentarAtualizarCobrancaComFormaPagamentoProcessandoDeveLancarExcecao()
        {
            Assert.Throws<ImpossivelAlterarCobrancaComFormaPagamentoPendenteException>(() => _cobrancaBoleto
                .AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoTentarAtualizarCobrancaComPagamentoDeveLancarExcecao()
        {
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(_cobrancaBoleto.Valor);
            Assert.Throws<ImpossivelAlterarCobrancaPagaException>(() => _cobrancaBoletoFormaPagamentoFinalizada
                .AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoRegerarFormaPagamentoDeveCriarNovaInstanciaFormaPagamentoProcessando()
        {
            var formaPagamentoAnterior = _cobrancaBoletoFormaPagamentoFinalizada.Transacao;
            _cobrancaBoletoFormaPagamentoFinalizada.ReprocessarTransacao();
            Assert.AreNotSame(formaPagamentoAnterior, _cobrancaBoletoFormaPagamentoFinalizada.Transacao);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Transacao.Status, StatusTransacao.Processando);
        }

        [Test]
        public void AoRegerarFormaPagamentoComPagamentoRealizadoDeveLancarExcecao()
        {
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(2000);
            Assert.Throws<ImpossivelRegerarFormaPagamentoParaCobrancaPagaException>(() => _cobrancaBoletoFormaPagamentoFinalizada.ReprocessarTransacao());
        }

        [Test]
        public void AoRegerarFormaPagamentoParaCobrancaComFormaPagamentoPendenteDeveLancarExcecao()
        {
            Assert.Throws<ImpossivelRegerarFormaQuandoFormaPagamentoPendenteException>(() => _cobrancaCartao.ReprocessarTransacao());
        }

        [Test]
        public void AoRegerarFormaPagamentoDeveAdicionarEventoCobranca()
        {
            var formaPagamentoEvent = _cobrancaBoletoFormaPagamentoFinalizada
                .Events
                .Where(e => e is TransacaoCobrancaReprocessandodoEvent)
                .Cast<TransacaoCobrancaReprocessandodoEvent>();

            Assert.IsNull(formaPagamentoEvent.SingleOrDefault());

            _cobrancaBoletoFormaPagamentoFinalizada.ReprocessarTransacao();

            Assert.AreEqual(formaPagamentoEvent.SingleOrDefault().CobrancaId, _cobrancaBoletoFormaPagamentoFinalizada.Id.ToString());
        }

        [Test]
        public void AoAlterarCobrancaDeveAdicionarEventoACobranca()
        {
            var cobrancaAlteradaEvent = _cobrancaBoletoFormaPagamentoFinalizada
                .Events.Where(e => e is CobrancaAlteradaEvent).Cast<CobrancaAlteradaEvent>();

            Assert.IsNull(cobrancaAlteradaEvent.SingleOrDefault());

            var vencimento = _cobrancaBoletoFormaPagamentoFinalizada.Vencimento;
            var configuracaoEmissaoId = _cobrancaBoletoFormaPagamentoFinalizada.ConfiguracaoEmissaoId;
            var valor = _cobrancaBoletoFormaPagamentoFinalizada.Valor;

            _cobrancaBoletoFormaPagamentoFinalizada.AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString());

            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().CobrancaId, _cobrancaBoletoFormaPagamentoFinalizada.Id.ToString());
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ValorAnterior, valor);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().VencimentoAnterior, vencimento);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ConfiguracaoEmissaoIdAnterior, configuracaoEmissaoId);
        }

        [Test]
        public void AoAlterarCobrancaBoletoDeveRegerarTransacao()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;

            var transacaoBoletoReprocessadaEvent = cobrancaBoleto.Events.Where(e => e is TransacaoCobrancaReprocessandodoEvent)
                    .Cast<TransacaoCobrancaReprocessandodoEvent>();

            Assert.IsNull(transacaoBoletoReprocessadaEvent.SingleOrDefault());

            cobrancaBoleto.AlterarCobranca(22, DateTime.Today, Guid.NewGuid().ToString());

            Assert.IsNotNull(transacaoBoletoReprocessadaEvent.SingleOrDefault());
        }

        [Test]
        public void AoAlterarCobrancaCartaoNaoDeveRegerarTransacao()
        {
            var cobrancaCartao = CobrancaBuilder.BuildCobrancaCartao().ComErroTransacao();

            var transacaoBoletoReprocessadaEvent = cobrancaCartao.Events.Where(e => e is TransacaoCobrancaReprocessandodoEvent)
                .Cast<TransacaoCobrancaReprocessandodoEvent>();

            Assert.IsNull(transacaoBoletoReprocessadaEvent.SingleOrDefault());

            cobrancaCartao.AlterarCobranca(22, DateTime.Today, Guid.NewGuid().ToString());

            Assert.IsNull(transacaoBoletoReprocessadaEvent.SingleOrDefault());
        }

        [Test]
        public void AoFinalizarProcessamentoFormaPagamentoCartaoDeveRealizarPagamento()
        {
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento();
            Assert.AreEqual(_cobrancaCartao.Pagamento?.Valor, _cobrancaCartao.Valor);
        }
    }

    public static class CobrancaBuilder
    {
        public static Cobranca ComErroTransacao(this Cobranca cobranca)
            => cobranca
                .ErroCriarTransacao();

        public static Cobranca ComTransacaoFinalizada(this Cobranca cobranca)
            => cobranca
                .FinalizaProcessamentoFormaPagamento();

        public static Cobranca BuildCobrancaBoleto() =>
            Cobranca.Boleto(valor: 200, vencimento: DateTime.Today, configuracaoEmissaoId: Guid.NewGuid().ToString(), "Teste Nome", "12345678912", "email@email.com", 
                new Telefone("12", "123456782"), 
                new Endereco("adasd", "132A", "123123Bairro", "1234568", "CE", "Teste"), 
                tenantIdCliente: Guid.NewGuid().ToString());

        public static Cobranca BuildCobrancaCartao() =>
            Cobranca.Cartao(valor: 200, vencimento: DateTime.Today, configuracaoEmissaoId: Guid.NewGuid().ToString(), 
                tenantIdCliente: Guid.NewGuid().ToString(), nomeCliente: "Teste Bla", cpfCnpjCliente: "44422255588", 
                emailCliente: "email@email.com", telefoneCliente: new Telefone("12", "55887744"), 
                cartaoCreditoCliente: new CartaoCredito("Teste nome", "1234", Guid.NewGuid().ToString()));

        public static Cobranca BuildCobrancaCartao(decimal valor, DateTime vencimento, string configuracaoEmissaoId,
            string nomeCliente, string cpfCnpjCliente, string emailCliente, Telefone telefoneCliente, CartaoCredito cartaoCreditoCliente, string tenantIdCliente)
            => Cobranca.Cartao(valor, vencimento, configuracaoEmissaoId, nomeCliente, cpfCnpjCliente, emailCliente, telefoneCliente, cartaoCreditoCliente, null, tenantIdCliente);

        public static Cobranca BuildCobrancaBoleto(decimal valor, DateTime vencimento, string clienteId,
            string configuracaoEmissaoId, string nomeCliente, string cpfCnpjCliente, string emailCliente,
            Telefone telefoneCliente, Endereco enderecoCliente, string tenantIdCliente)
            => Cobranca.Boleto(valor, vencimento, configuracaoEmissaoId, nomeCliente, cpfCnpjCliente, emailCliente, telefoneCliente, enderecoCliente, tenantIdCliente);
    }
}