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
        private Cobranca _cobrancaFormaPagamentoFinalizada;

        [SetUp]
        public void Setup()
        {
            _cobrancaBoleto = CobrancaBuilder.BuildCobrancaBoleto();
            _cobrancaCartao = CobrancaBuilder.BuildCobrancaCartao();
            _cobrancaFormaPagamentoFinalizada = CobrancaBuilder
                .BuildCobrancaBoleto()
                .ComFormaPagamentoFinalizada();
        }

        [Test]
        public void ConsigoCriarCobrancaDeCartao()
        {
            Assert.DoesNotThrow(() => CobrancaBuilder.BuildCobrancaCartao());
        }

        [Test]
        public void ConsigoCriarCobrancaDeBoleto()
        {
            Assert.DoesNotThrow(() => CobrancaBuilder.BuildCobrancaBoleto());
        }

        [Test]
        public void NaoConsigoAlterarAFormaDePagamentoCasoAindaEstejaProcessando()
        {
            Assert.Throws<FormaPagamentoAindaEmProcessamentoException>(() => _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao(Guid.NewGuid().ToString())));
            Assert.Throws<FormaPagamentoAindaEmProcessamentoException>(() => _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto(Guid.NewGuid().ToString())));
        }

        [Test]
        public void ConsigoAlterarAFormaDePagamentoCasoFormaPagamentAtualNaoEstejaProcessando()
        {
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento();
            _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();

            Assert.DoesNotThrow(() => _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao(Guid.NewGuid().ToString())));
            Assert.DoesNotThrow(() => _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto(Guid.NewGuid().ToString())));
        }

        [Test]
        public void AoAlterarFormaPagamentoDeveCriarEventoNaCobranca()
        {
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento();
            _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();

            Assert.AreEqual(_cobrancaCartao.Events.Count(e => e is FormaPagamentoAlteradaEvent), 0);
            Assert.AreEqual(_cobrancaBoleto.Events.Count(e => e is FormaPagamentoAlteradaEvent), 0);

            var formaPagamentoCartaoAnterior = _cobrancaCartao.FormaPagamento;
            var formaPagamentoBoletoAnterior = _cobrancaBoleto.FormaPagamento;

            _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao(Guid.NewGuid().ToString()));
            _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto(Guid.NewGuid().ToString()));

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
            Assert.AreEqual(cobrancaBoletoEvent?.Cobranca, _cobrancaBoleto);
            Assert.AreEqual(cobrancaCartaoEvent?.Cobranca, _cobrancaCartao);
        }

        [Test]
        public void AoCriarUmaCobrancaDeveCriarEventoNaCobranca()
        {
            Assert.AreEqual(CobrancaBuilder.BuildCobrancaCartao().Events.Count(e => e is CobrancaCriadaEvent), 1);
            Assert.AreEqual(CobrancaBuilder.BuildCobrancaBoleto().Events.Count(e => e is CobrancaCriadaEvent), 1);
        }

        [Test]
        public void StatusProcessamentoDeveSerIgualDeAcordoComSolicitado([Values(StatusFormaPagamento.Processando, 
                StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            var cobranca = _cobrancaBoleto;

            if (statusFormaPagamento == StatusFormaPagamento.Processando)
            {
                cobranca = CobrancaBuilder.BuildCobrancaCartao();
            } else if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();
            } else if (statusFormaPagamento == StatusFormaPagamento.Erro)
            {
                _cobrancaBoleto.ErroCriarFormaPagamento();
            }

            Assert.AreEqual(cobranca.FormaPagamento.Status, statusFormaPagamento);
        }

        [Test]
        public void AoTentarMudarStatusFormaPagamentoQuandoStatusNaoForPermitidoDeveLancarExcecao([Values(StatusFormaPagamento.Processando,
            StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento());
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.ErroCriarFormaPagamento());
            } else if (statusFormaPagamento == StatusFormaPagamento.Erro)
            {
                _cobrancaBoleto.ErroCriarFormaPagamento();
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento());
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.ErroCriarFormaPagamento());
            }
        }

        [Test]
        public void AoMudarStatusFormaPagamentoDeveAdicionarEventoNaCobranca([Values(StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento();

                Assert.AreEqual(_cobrancaBoleto.Events
                    .Where(e => e is FormaPagamentoProcessadaEvent)
                    .Cast<FormaPagamentoProcessadaEvent>()
                    .SingleOrDefault()?.Cobranca, _cobrancaBoleto);
            }
            else if (statusFormaPagamento == StatusFormaPagamento.Erro)
            {
                _cobrancaBoleto.ErroCriarFormaPagamento();

                Assert.AreEqual(_cobrancaBoleto.Events
                    .Where(e => e is FalhaAoProcessarFormaPagamentoEvent)
                    .Cast<FalhaAoProcessarFormaPagamentoEvent>()
                    .SingleOrDefault()?.Cobranca, _cobrancaBoleto);
            }
        }

        [Test]
        public void AoAdicionarPagamentoDeveAdicionarEventoNaCobranca()
        {
            _cobrancaFormaPagamentoFinalizada.RealizarPagamento(200);
            Assert.AreEqual(_cobrancaFormaPagamentoFinalizada.Events
                .Where(e => e is PagamentoRealizadoEvent)
                .Cast<PagamentoRealizadoEvent>()
                .SingleOrDefault()?.Cobranca, _cobrancaFormaPagamentoFinalizada);
        }

        [Test]
        public void ConsigoFinalizarUmProcessamentoFormaPagamento()
        {
            Assert.DoesNotThrow(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento());
        }

        [Test]
        public void ConsigoDefinirComoErroUmProcessamentoFormaPagamento()
        {
            Assert.DoesNotThrow(() => _cobrancaBoleto.ErroCriarFormaPagamento());
        }

        [Test]
        public void ConsigoRealizarPagamentoCobranca()
        {
            _cobrancaFormaPagamentoFinalizada.RealizarPagamento(200);
            _cobrancaFormaPagamentoFinalizada.RealizarPagamento(200);
        }

        [Test]
        public void DeveLancarExcecaoAoTentarRealizarPagamentoComFormaPagamentoNaoFinalizada()
        {
            Assert.Throws<FormaPagamentoNaoProcessadaException>(() => _cobrancaCartao.RealizarPagamento(200));
            _cobrancaCartao.ErroCriarFormaPagamento();
            Assert.Throws<FormaPagamentoNaoProcessadaException>(() => _cobrancaCartao.RealizarPagamento(200));
        }

        [Test]
        public void AoAlterarCobrancaOsNovosValoresDevemEstarSetadosCorretamente()
        {
            var vencimento = DateTime.Today;
            var contaBancariaId = Guid.NewGuid().ToString();
            var emissorId = Guid.NewGuid().ToString();
            var pagadorId = Guid.NewGuid().ToString();
            var valor = 20;

            _cobrancaFormaPagamentoFinalizada.AlterarCobranca(valor, vencimento, emissorId, pagadorId, contaBancariaId);

            Assert.AreEqual(_cobrancaFormaPagamentoFinalizada.Valor, valor);
            Assert.AreEqual(_cobrancaFormaPagamentoFinalizada.PagadorId, pagadorId);
            Assert.AreEqual(_cobrancaFormaPagamentoFinalizada.Vencimento, vencimento);
            Assert.AreEqual(_cobrancaFormaPagamentoFinalizada.ContaBancariaId, contaBancariaId);
            Assert.AreEqual(_cobrancaFormaPagamentoFinalizada.EmissorId, emissorId);
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
            _cobrancaFormaPagamentoFinalizada.RealizarPagamento(_cobrancaBoleto.Valor);
            Assert.Throws<ImpossivelAlterarCobrancaPagaException>(() => _cobrancaFormaPagamentoFinalizada
                .AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoRegerarFormaPagamentoDeveCriarNovaInstanciaFormaPagamento()
        {
            var formaPagamentoAnterior = _cobrancaFormaPagamentoFinalizada.FormaPagamento;
            _cobrancaFormaPagamentoFinalizada.RegerarFormaPagamento(Guid.NewGuid().ToString());
            Assert.AreNotSame(formaPagamentoAnterior, _cobrancaFormaPagamentoFinalizada.FormaPagamento);
        }

        [Test]
        public void AoRegerarFormaPagamentoComPagamentoRealizadoDeveLancarExcecao()
        {
            _cobrancaFormaPagamentoFinalizada.RealizarPagamento(2000);
            Assert.Throws<ImpossivelRegerarFormaPagamentoParaCobrancaPagaException>(() => _cobrancaFormaPagamentoFinalizada.RegerarFormaPagamento(Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoRegerarFormaPagamentoParaCobrancaComFormaPagamentoPendenteDeveLancarExcecao()
        {
            Assert.Throws<ImpossivelRegerarFormaQuandoFormaPagamentoPendenteException>(() => _cobrancaCartao.RegerarFormaPagamento(Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoRegerarFormaPagamentoDeveAdicionarEventoCobranca()
        {
            var formaPagamentoEvent = _cobrancaFormaPagamentoFinalizada
                .Events
                .Where(e => e is FormaPagamentoRegeradaEvent)
                .Cast<FormaPagamentoRegeradaEvent>();

            Assert.IsNull(formaPagamentoEvent.SingleOrDefault());

            var formaPagamentoAnterior = _cobrancaFormaPagamentoFinalizada.FormaPagamento;

            _cobrancaFormaPagamentoFinalizada.RegerarFormaPagamento(Guid.NewGuid().ToString());

            Assert.AreSame(formaPagamentoEvent.SingleOrDefault().FormaPagamentoAnterior, formaPagamentoAnterior);
            Assert.AreSame(formaPagamentoEvent.SingleOrDefault().FormaPagamento, _cobrancaFormaPagamentoFinalizada.FormaPagamento);
        }

        [Test]
        public void AoAlterarCobrancaDeveAdicionarEventoACobranca()
        {
            var cobrancaAlteradaEvent = _cobrancaFormaPagamentoFinalizada
                .Events.Where(e => e is CobrancaAlteradaEvent).Cast<CobrancaAlteradaEvent>();

            Assert.IsNull(cobrancaAlteradaEvent.SingleOrDefault());

            var vencimento = _cobrancaFormaPagamentoFinalizada.Vencimento;
            var contaBancariaId = _cobrancaFormaPagamentoFinalizada.ContaBancariaId;
            var emissorId = _cobrancaFormaPagamentoFinalizada.EmissorId;
            var pagadorId = _cobrancaFormaPagamentoFinalizada.PagadorId;
            var valor = _cobrancaFormaPagamentoFinalizada.Valor;

            _cobrancaFormaPagamentoFinalizada.AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().Cobranca, _cobrancaFormaPagamentoFinalizada);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ValorAnterior, valor);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().VencimentoAnterior, vencimento);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().ContaBancariaIdAnterior, contaBancariaId);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().EmissorIdAnterior, emissorId);
            Assert.AreEqual(cobrancaAlteradaEvent.SingleOrDefault().PagadorIdAnterior, pagadorId);
        }
    }

    public static class CobrancaBuilder
    {
        public static Cobranca ComFormaPagamentoFinalizada(this Cobranca cobranca)
            => cobranca
                .FinalizaProcessamentoFormaPagamento();

        public static Cobranca BuildCobrancaBoleto() =>
            Cobranca.Boleto(valor: 200, vencimento: DateTime.Today,
                pagadorId: Guid.NewGuid().ToString(), emissorId: Guid.NewGuid().ToString(),
                contaBancariaId: Guid.NewGuid().ToString(), cobrancaBoletoId: Guid.NewGuid().ToString());

        public static Cobranca BuildCobrancaCartao() =>
            Cobranca.Cartao(valor: 200, vencimento: DateTime.Today,
                pagadorId: Guid.NewGuid().ToString(), emissorId: Guid.NewGuid().ToString(),
                contaBancariaId: Guid.NewGuid().ToString(), cobrancaCartaoId: Guid.NewGuid().ToString());
    }
}