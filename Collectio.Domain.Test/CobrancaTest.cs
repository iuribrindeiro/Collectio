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
            var emissorId = Guid.NewGuid().ToString();
            var pagadorId = Guid.NewGuid();
            var contaBancariaId = Guid.NewGuid().ToString();

            var novaCobranca = CobrancaBuilder.BuildCobrancaCartao(valor, vencimento, pagadorId, emissorId, contaBancariaId);

            Assert.AreEqual(novaCobranca.Valor, valor);
            Assert.AreEqual(novaCobranca.Vencimento, vencimento);
            Assert.AreEqual(novaCobranca.EmissorId, emissorId);
            Assert.AreEqual(novaCobranca.PagadorId, pagadorId);
            Assert.AreEqual(novaCobranca.ContaBancariaId, contaBancariaId);
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
            var contaBancariaId = Guid.NewGuid().ToString();
            var emissorId = Guid.NewGuid().ToString();
            var pagadorId = Guid.NewGuid();
            var valor = 20;

            _cobrancaBoletoFormaPagamentoFinalizada.AlterarCobranca(valor, vencimento, emissorId, pagadorId, contaBancariaId);

            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Valor, valor);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.PagadorId, pagadorId);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Vencimento, vencimento);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.ContaBancariaId, contaBancariaId);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.EmissorId, emissorId);
        }

        [Test]
        public void AoTentarAtualizarCobrancaComFormaPagamentoProcessandoDeveLancarExcecao()
        {
            Assert.Throws<ImpossivelAlterarCobrancaComFormaPagamentoPendenteException>(() => _cobrancaBoleto
                .AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoTentarAtualizarCobrancaComPagamentoDeveLancarExcecao()
        {
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(_cobrancaBoleto.Valor);
            Assert.Throws<ImpossivelAlterarCobrancaPagaException>(() => _cobrancaBoletoFormaPagamentoFinalizada
                .AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid().ToString()));
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
                .Where(e => e is TransacaoCobrancaReprocessadaEvent)
                .Cast<TransacaoCobrancaReprocessadaEvent>();

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
            var contaBancariaId = _cobrancaBoletoFormaPagamentoFinalizada.ContaBancariaId;
            var emissorId = _cobrancaBoletoFormaPagamentoFinalizada.EmissorId;
            var pagadorId = _cobrancaBoletoFormaPagamentoFinalizada.PagadorId;
            var valor = _cobrancaBoletoFormaPagamentoFinalizada.Valor;

            _cobrancaBoletoFormaPagamentoFinalizada.AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid().ToString());

            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().Cobranca, _cobrancaBoletoFormaPagamentoFinalizada);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ValorAnterior, valor);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().VencimentoAnterior, vencimento);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ContaBancariaIdAnterior, contaBancariaId);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().EmissorIdAnterior, emissorId);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().PagadorIdAnterior, pagadorId);
        }


        [Test]
        public void AoAlterarValorCobrancaDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartao.ComErroTransacao();
            var valor = new Random().Next(201, 2000);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(valor, cobrancaBoleto.Vencimento, cobrancaBoleto.EmissorId, cobrancaBoleto.PagadorId, cobrancaBoleto.ContaBancariaId, cobrancaBoleto);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(valor, cobrancaCartao.Vencimento, cobrancaCartao.EmissorId, cobrancaCartao.PagadorId, cobrancaCartao.ContaBancariaId, cobrancaCartao);
        }

        [Test]
        public void AoAlterarEmissorCobrancaDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartao.ComErroTransacao();
            var emissorId = Guid.NewGuid().ToString();
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, emissorId, cobrancaBoleto.PagadorId, cobrancaBoleto.ContaBancariaId, cobrancaBoleto);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, cobrancaCartao.Vencimento, emissorId, cobrancaCartao.PagadorId, cobrancaCartao.ContaBancariaId, cobrancaCartao);
        }

        [Test]
        public void AoAlterarPagadorCobrancaDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartao.ComErroTransacao();
            var pagadorId = Guid.NewGuid();
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, cobrancaBoleto.EmissorId, pagadorId, cobrancaBoleto.ContaBancariaId, cobrancaBoleto);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, cobrancaCartao.Vencimento, cobrancaCartao.EmissorId, pagadorId, cobrancaCartao.ContaBancariaId, cobrancaCartao);
        }

        [Test]
        public void AoAlterarContaBancariaCobrancaDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartao.ComErroTransacao();
            var contaBancariaId = Guid.NewGuid().ToString();
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, cobrancaBoleto.EmissorId, cobrancaBoleto.PagadorId, contaBancariaId, cobrancaBoleto);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, cobrancaCartao.Vencimento, cobrancaCartao.EmissorId, cobrancaCartao.PagadorId, contaBancariaId, cobrancaCartao);
        }

        [Test]
        public void AoAlterarCobrancaSemNovosValoresNaoDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartao.ComErroTransacao();
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, cobrancaBoleto.EmissorId, cobrancaBoleto.PagadorId, cobrancaBoleto.ContaBancariaId, cobrancaBoleto, false);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, cobrancaCartao.Vencimento, cobrancaCartao.EmissorId, cobrancaCartao.PagadorId, cobrancaCartao.ContaBancariaId, cobrancaCartao, false);
        }

        [Test]
        public void AoAlterarVencimentoCobrancaCartaoNaoDeveRegerarFormaPagamento()
        {
            var gen = new Random();
            var cobrancaCartao = _cobrancaCartao.ComErroTransacao();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, start.AddDays(gen.Next(range)), cobrancaCartao.EmissorId, cobrancaCartao.PagadorId, 
                cobrancaCartao.ContaBancariaId, cobrancaCartao, validaRegeraFormaPagamento: false);
        }

        [Test]
        public void AoFinalizarProcessamentoFormaPagamentoCartaoDeveRealizarPagamento()
        {
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento();
            Assert.AreEqual(_cobrancaCartao.Pagamento?.Valor, _cobrancaCartao.Valor);
        }

        private void TestaRegeraFormaPagamentoNaAlteracaoCobranca(decimal valor, DateTime vencimento, string emissorId, Guid pagadorId, string contaBancariaId, Cobranca cobranca, bool validaRegeraFormaPagamento = true)
        {
            var formaPagamentoAnterior = cobranca.Transacao;

            var formaPagamentoRegeradaEvent = cobranca.Events.Where(e => e is TransacaoCobrancaReprocessadaEvent)
                .Cast<TransacaoCobrancaReprocessadaEvent>();

            Assert.IsNull(formaPagamentoRegeradaEvent.SingleOrDefault());

            cobranca.AlterarCobranca(valor, vencimento, emissorId, pagadorId, contaBancariaId);

            if (validaRegeraFormaPagamento)
            {
                Assert.AreEqual(cobranca.Transacao.Status, StatusTransacao.Processando);
                Assert.AreEqual(formaPagamentoRegeradaEvent.SingleOrDefault()?.CobrancaId, cobranca.Id.ToString());
            }
            else
            {
                Assert.IsNull(formaPagamentoRegeradaEvent.SingleOrDefault());
                Assert.AreSame(cobranca.Transacao, formaPagamentoAnterior);
                Assert.AreEqual(cobranca.Transacao.Status, formaPagamentoAnterior.Status);
            }
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
                pagadorId: Guid.NewGuid(), emissorId: Guid.NewGuid().ToString(),
                contaBancariaId: Guid.NewGuid().ToString());

        public static Cobranca BuildCobrancaCartao() =>
            Cobranca.Cartao(valor: 200, vencimento: DateTime.Today,
                pagadorId: Guid.NewGuid(), emissorId: Guid.NewGuid().ToString(),
                contaBancariaId: Guid.NewGuid().ToString());

        public static Cobranca BuildCobrancaCartao(decimal valor, DateTime vencimento, Guid pagadorId, string emissorId, string contaBancariaId)
            => Cobranca.Cartao(valor, vencimento, pagadorId, emissorId, contaBancariaId);

        public static Cobranca BuildCobrancaBoleto(decimal valor, DateTime vencimento, Guid pagadorId, string emissorId, string contaBancariaId)
            => Cobranca.Boleto(valor, vencimento, pagadorId, emissorId, contaBancariaId);
    }
}