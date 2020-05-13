using System;
using System.Linq;
using System.Runtime.InteropServices;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.AjustesValorPagamento;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.CobrancaAggregate.Exceptions;
using NUnit.Framework;

namespace Collectio.Domain.Test
{
    public class CobrancaTest
    {
        private Cobranca _cobrancaBoleto;
        private Cobranca _cobrancaCartao;

        [SetUp]
        public void Setup()
        {
            _cobrancaBoleto = CobrancaBuilder.BuildCobrancaBoleto();
            _cobrancaCartao = CobrancaBuilder.BuildCobrancaCartao();
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
        public void NaoConsigoAlterarAFormaDePagamentoCasoAindaEstejaAguardandoInicioProcessamento()
        {
            Assert.Throws<FormaPagamentoAindaEmProcessamentoException>(() => _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao()));
            Assert.Throws<FormaPagamentoAindaEmProcessamentoException>(() => _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto()));
        }

        [Test]
        public void NaoConsigoAlterarAFormaDePagamentoCasoAindaEstejaProcessando()
        {
            _cobrancaCartao.IniciarProcessamentoFormaPagamento();
            _cobrancaBoleto.IniciarProcessamentoFormaPagamento();

            Assert.Throws<FormaPagamentoAindaEmProcessamentoException>(() => _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao()));
            Assert.Throws<FormaPagamentoAindaEmProcessamentoException>(() => _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto()));
        }

        [Test]
        public void ConsigoAlterarAFormaDePagamentoCasoFormaPagamentAtualNaoEstejaProcessando()
        {
            _cobrancaCartao.IniciarProcessamentoFormaPagamento();
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());

            _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
            _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());

            Assert.DoesNotThrow(() => _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao()));
            Assert.DoesNotThrow(() => _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto()));
        }

        [Test]
        public void AoAlterarFormaPagamentoDeveCriarEventoNaCobranca()
        {
            _cobrancaCartao.IniciarProcessamentoFormaPagamento();
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());

            _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
            _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());

            Assert.AreEqual(_cobrancaCartao.Events.Count(e => e is FormaPagamentoAlteradaEvent), 0);
            Assert.AreEqual(_cobrancaBoleto.Events.Count(e => e is FormaPagamentoAlteradaEvent), 0);

            var formaPagamentoCartaoAnterior = _cobrancaCartao.FormaPagamento;
            var formaPagamentoBoletoAnterior = _cobrancaBoleto.FormaPagamento;

            _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao());
            _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto());

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
        public void StatusProcessamentoDeveSerIgualDeAcordoComSolicitado([Values(
                StatusFormaPagamento.AguardandoInicioProcessamento, StatusFormaPagamento.Processando, 
                StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            var cobranca = _cobrancaBoleto;

            if (statusFormaPagamento == StatusFormaPagamento.AguardandoInicioProcessamento)
            {
                cobranca = CobrancaBuilder.BuildCobrancaCartao();
            }
            else if (statusFormaPagamento == StatusFormaPagamento.Processando)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
            } else if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());
            } else if (statusFormaPagamento == StatusFormaPagamento.Erro)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
                _cobrancaBoleto.ErroCriarFormaPagamento();
            }

            Assert.AreEqual(cobranca.FormaPagamento.Status, statusFormaPagamento);
        }

        [Test]
        public void AoTentarMudarStatusFormaPagamentoQuandoStatusNaoForPermitidoDeveLancarExcecao([Values(StatusFormaPagamento.AguardandoInicioProcessamento, StatusFormaPagamento.Processando,
            StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            if (statusFormaPagamento == StatusFormaPagamento.AguardandoInicioProcessamento)
            {
                Assert.Throws<FormaPagamentoAguardandoInicioProcessamentoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString()));
                Assert.Throws<FormaPagamentoAguardandoInicioProcessamentoException>(() => _cobrancaBoleto.ErroCriarFormaPagamento());
            }
            else if (statusFormaPagamento == StatusFormaPagamento.Processando)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
                Assert.Throws<ProcessoFormaPagamentoJaIniciadoException>(() => _cobrancaBoleto.IniciarProcessamentoFormaPagamento());
                
            } else if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString()));
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.IniciarProcessamentoFormaPagamento());
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.ErroCriarFormaPagamento());
            } else if (statusFormaPagamento == StatusFormaPagamento.Erro)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
                _cobrancaBoleto.ErroCriarFormaPagamento();

                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString()));
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.ErroCriarFormaPagamento());
            }
        }

        [Test]
        public void AoMudarStatusFormaPagamentoDeveAdicionarEventoNaCobranca([Values(StatusFormaPagamento.Processando,
            StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            if (statusFormaPagamento == StatusFormaPagamento.Processando)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
                Assert.AreEqual(_cobrancaBoleto.Events
                    .Where(e => e is IniciadoProcessamentoFormaPagamentoEvent)
                    .Cast<IniciadoProcessamentoFormaPagamentoEvent>()
                    .SingleOrDefault()?.Cobranca, _cobrancaBoleto);
            }
            else if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());

                Assert.AreEqual(_cobrancaBoleto.Events
                    .Where(e => e is FormaPagamentoProcessadaEvent)
                    .Cast<FormaPagamentoProcessadaEvent>()
                    .SingleOrDefault()?.Cobranca, _cobrancaBoleto);
            }
            else if (statusFormaPagamento == StatusFormaPagamento.Erro)
            {
                _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
                _cobrancaBoleto.ErroCriarFormaPagamento();

                Assert.AreEqual(_cobrancaBoleto.Events
                    .Where(e => e is FalhaAoProcessarFormaPagamentoEvent)
                    .Cast<FalhaAoProcessarFormaPagamentoEvent>()
                    .SingleOrDefault()?.Cobranca, _cobrancaBoleto);
            }
        }

        [Test]
        public void ConsigoFinalizarUmProcessamentoFormaPagamento()
        {
            _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
            Assert.DoesNotThrow(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString()));
        }

        [Test]
        public void ConsigoDefinirComoErroUmProcessamentoFormaPagamento()
        {
            _cobrancaBoleto.IniciarProcessamentoFormaPagamento();
            Assert.DoesNotThrow(() => _cobrancaBoleto.ErroCriarFormaPagamento());
        }

        public class CobrancaBuilder
        {
            public static Cobranca BuildCobrancaBoleto() =>
                Cobranca.Boleto(valor: 200, vencimento: DateTime.Today,
                    pagadorId: Guid.NewGuid().ToString(), emissorId: Guid.NewGuid().ToString(),
                    contaBancariaId: Guid.NewGuid().ToString());

            public static Cobranca BuildCobrancaCartao() =>
                Cobranca.Cartao(valor: 200, vencimento: DateTime.Today,
                    pagadorId: Guid.NewGuid().ToString(), emissorId: Guid.NewGuid().ToString(),
                    contaBancariaId: Guid.NewGuid().ToString());
        }
    }
}