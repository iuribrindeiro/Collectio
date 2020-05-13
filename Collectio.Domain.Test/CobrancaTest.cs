using System;
using System.Linq;
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
        public void NaoConsigoAlterarAFormaDePagamentoCasoAindaEstejaIniciandoProcessamento()
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

            _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao());
            _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto());

            Assert.AreEqual(_cobrancaCartao.Events.Count(e => e is FormaPagamentoAlteradaEvent), 1);
            Assert.AreEqual(_cobrancaBoleto.Events.Count(e => e is FormaPagamentoAlteradaEvent), 1);
        }

        [Test]
        public void AoCriarUmaCobrancaDeveCriarEventoNaCobranca()
        {
            Assert.AreEqual(CobrancaBuilder.BuildCobrancaCartao().Events.Count(e => e is CobrancaCriadaEvent), 1);
            Assert.AreEqual(CobrancaBuilder.BuildCobrancaBoleto().Events.Count(e => e is CobrancaCriadaEvent), 1);
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