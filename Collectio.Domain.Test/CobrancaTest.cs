using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.CobrancaAggregate.Exceptions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Collectio.Domain.Test
{
    public class CobrancaTest
    {
        private Cobranca _cobrancaBoleto;
        private Cobranca _cobrancaCartao;
        private Cobranca _cobrancaBoletoFormaPagamentoFinalizada;
        private Cobranca _cobrancaCartaoFormaPagamentoFinalizada;

        [SetUp]
        public void Setup()
        {
            _cobrancaBoleto = CobrancaBuilder.BuildCobrancaBoleto();
            _cobrancaCartao = CobrancaBuilder.BuildCobrancaCartao();
            _cobrancaBoletoFormaPagamentoFinalizada = CobrancaBuilder
                .BuildCobrancaBoleto()
                .ComTransacaoFinalizada();
            _cobrancaCartaoFormaPagamentoFinalizada = CobrancaBuilder
                .BuildCobrancaCartao()
                .ComTransacaoFinalizada();
        }

        [Test]
        public void AoCriarCobrancaCartaoOsValoresDevemSerSetadosCorretamente()
        {
            var valor = 20;
            var vencimento = DateTime.Today.AddDays(10);
            var clienteId = Guid.NewGuid().ToString();
            var cartaoCreditoId = Guid.NewGuid().ToString();
            var configuracaoEmissaoId = Guid.NewGuid().ToString();

            var novaCobranca = CobrancaBuilder.BuildCobrancaCartao(valor, vencimento, clienteId, configuracaoEmissaoId, cartaoCreditoId);

            Assert.AreEqual(novaCobranca.Valor, valor);
            Assert.AreEqual(novaCobranca.Vencimento, vencimento);
            Assert.AreEqual(novaCobranca.Pagador.ClienteId, clienteId);
            Assert.AreEqual(novaCobranca.Pagador.CartaoCreditoId, cartaoCreditoId);
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
            var cartaoCreditoId = Guid.NewGuid().ToString();
            var valor = 20;

            _cobrancaBoletoFormaPagamentoFinalizada.AlterarCobranca(valor, vencimento, clienteId, configuracaoEmissaoId, cartaoCreditoId);

            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Valor, valor);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Pagador.ClienteId, clienteId);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Pagador.CartaoCreditoId, cartaoCreditoId);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Vencimento, vencimento);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.ConfiguracaoEmissaoId, configuracaoEmissaoId);
        }

        [Test]
        public void AoTentarAtualizarCobrancaComFormaPagamentoProcessandoDeveLancarExcecao()
        {
            Assert.Throws<ImpossivelAlterarCobrancaComFormaPagamentoPendenteException>(() => _cobrancaBoleto
                .AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoTentarAtualizarCobrancaComPagamentoDeveLancarExcecao()
        {
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(_cobrancaBoleto.Valor);
            Assert.Throws<ImpossivelAlterarCobrancaPagaException>(() => _cobrancaBoletoFormaPagamentoFinalizada
                .AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
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
            var clienteId = _cobrancaBoletoFormaPagamentoFinalizada.Pagador.ClienteId;
            var cartaoCreditoId = _cobrancaBoletoFormaPagamentoFinalizada.Pagador.CartaoCreditoId;
            var valor = _cobrancaBoletoFormaPagamentoFinalizada.Valor;

            _cobrancaBoletoFormaPagamentoFinalizada.AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().CobrancaId, _cobrancaBoletoFormaPagamentoFinalizada.Id.ToString());
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ValorAnterior, valor);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().VencimentoAnterior, vencimento);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ContaBancariaIdAnterior, configuracaoEmissaoId);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ClienteIdAnterior, clienteId);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().CartaoCreditoIdAnterior, cartaoCreditoId);
        }

        [Test, Sequential]
        public void AoAlterarCobrancaSeAlgumCampoMudarDeveRegerarTransacao(
            [Values(200, 100)] decimal valor, [Values("2020-20-02", "2020-07-02")] string dataVencimento,
            [Values("123", "1234")] string clienteId,
            [Values("123", "1234")] string configuracaoEmissaoId,
            [Values("123", "1234")] string cartaoCreditoId)
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;

            var transacaoBoletoReprocessadaEvent = cobrancaBoleto.Events.Where(e => e is TransacaoCobrancaReprocessandodoEvent)
                    .Cast<TransacaoCobrancaReprocessandodoEvent>();

            Assert.IsNull(transacaoBoletoReprocessadaEvent.SingleOrDefault());

            cobrancaBoleto.AlterarCobranca(valor,
                DateTime.ParseExact(dataVencimento, "yyyy-dd-MM", null), clienteId, configuracaoEmissaoId,
                cartaoCreditoId);

            Assert.IsNotNull(transacaoBoletoReprocessadaEvent.SingleOrDefault());
        }

        [Test]
        public void AoAlterarCobrancaSeNenhumCampoMudarNaoDeveRegerarTransacao()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;

            var transacaoBoletoReprocessadaEvent = cobrancaBoleto.Events.Where(e => e is TransacaoCobrancaReprocessandodoEvent)
                .Cast<TransacaoCobrancaReprocessandodoEvent>();

            cobrancaBoleto.AlterarCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, cobrancaBoleto.Pagador.ClienteId, cobrancaBoleto.ConfiguracaoEmissaoId);

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
            Cobranca.Boleto(valor: 200, vencimento: DateTime.Today,
                clienteId: Guid.NewGuid().ToString(), configuracaoEmissaoId: Guid.NewGuid().ToString());

        public static Cobranca BuildCobrancaCartao() =>
            Cobranca.Cartao(valor: 200, vencimento: DateTime.Today,
                clienteId: Guid.NewGuid().ToString(), cartaoCreditoId: Guid.NewGuid().ToString(),
                configuracaoEmissaoId: Guid.NewGuid().ToString());

        public static Cobranca BuildCobrancaCartao(decimal valor, DateTime vencimento, string clienteId, string configuracaoEmissaoId, string cartaoCreditoId)
            => Cobranca.Cartao(valor, vencimento, clienteId, cartaoCreditoId, configuracaoEmissaoId);

        public static Cobranca BuildCobrancaBoleto(decimal valor, DateTime vencimento, string clienteId, string configuracaoEmissaoId)
            => Cobranca.Boleto(valor, vencimento, clienteId, configuracaoEmissaoId);
    }
}