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
                .ComFormaPagamentoFinalizada();
            _cobrancaCartaoFormaPagamentoFinalizada = CobrancaBuilder
                .BuildCobrancaCartao()
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
            Assert.Throws<FormaPagamentoAindaEmProcessamentoException>(() => _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao()));
            Assert.Throws<FormaPagamentoAindaEmProcessamentoException>(() => _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto()));
        }

        [Test]
        public void ConsigoAlterarAFormaDePagamentoCasoFormaPagamentAtualNaoEstejaProcessando()
        {
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());
            _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());

            Assert.DoesNotThrow(() => _cobrancaBoleto.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Cartao()));
            Assert.DoesNotThrow(() => _cobrancaCartao.AlterarFormaPagamento(Cobranca.FormaPagamentoValueObject.Boleto()));
        }

        [Test]
        public void AoAlterarFormaPagamentoDeveCriarEventoNaCobranca()
        {
            _cobrancaCartao.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());
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
        public void StatusProcessamentoDeveSerIgualDeAcordoComSolicitado([Values(StatusFormaPagamento.Processando, 
                StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            var cobranca = _cobrancaBoleto;

            if (statusFormaPagamento == StatusFormaPagamento.Processando)
            {
                cobranca = CobrancaBuilder.BuildCobrancaCartao();
            } else if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());
            } else if (statusFormaPagamento == StatusFormaPagamento.Erro)
            {
                _cobrancaBoleto.ErroCriarFormaPagamento();
            }

            Assert.AreEqual(cobranca.FormaPagamento.Status, statusFormaPagamento);
        }

        [Test]
        public void AoFinalizarProcessamentoFormaPagamentoIdDeveSerSetadoCorretamente()
        {
            var id = Guid.NewGuid().ToString();
            _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(id);
            Assert.AreEqual(id, _cobrancaBoleto.FormaPagamento.Id);
        }

        [Test]
        public void AoTentarMudarStatusFormaPagamentoQuandoStatusNaoForPermitidoDeveLancarExcecao([Values(StatusFormaPagamento.Processando,
            StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString()));
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.ErroCriarFormaPagamento());
            } else if (statusFormaPagamento == StatusFormaPagamento.Erro)
            {
                _cobrancaBoleto.ErroCriarFormaPagamento();
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString()));
                Assert.Throws<ProcessoFormaPagamentoJaFinalizadoException>(() => _cobrancaBoleto.ErroCriarFormaPagamento());
            }
        }

        [Test]
        public void AoMudarStatusFormaPagamentoDeveAdicionarEventoNaCobranca([Values(StatusFormaPagamento.Criado, StatusFormaPagamento.Erro)] StatusFormaPagamento statusFormaPagamento)
        {
            if (statusFormaPagamento == StatusFormaPagamento.Criado)
            {
                _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());

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
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(200);
            Assert.AreEqual(_cobrancaBoletoFormaPagamentoFinalizada.Events
                .Where(e => e is PagamentoRealizadoEvent)
                .Cast<PagamentoRealizadoEvent>()
                .SingleOrDefault()?.Cobranca, _cobrancaBoletoFormaPagamentoFinalizada);
        }

        [Test]
        public void ConsigoFinalizarUmProcessamentoFormaPagamento()
        {
            Assert.DoesNotThrow(() => _cobrancaBoleto.FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString()));
        }

        [Test]
        public void ConsigoDefinirComoErroUmProcessamentoFormaPagamento()
        {
            Assert.DoesNotThrow(() => _cobrancaBoleto.ErroCriarFormaPagamento());
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
        public void AoRegerarFormaPagamentoDeveCriarNovaInstanciaFormaPagamento()
        {
            var formaPagamentoAnterior = _cobrancaBoletoFormaPagamentoFinalizada.FormaPagamento;
            _cobrancaBoletoFormaPagamentoFinalizada.RegerarFormaPagamento();
            Assert.AreNotSame(formaPagamentoAnterior, _cobrancaBoletoFormaPagamentoFinalizada.FormaPagamento);
        }

        [Test]
        public void AoRegerarFormaPagamentoComPagamentoRealizadoDeveLancarExcecao()
        {
            _cobrancaBoletoFormaPagamentoFinalizada.RealizarPagamento(2000);
            Assert.Throws<ImpossivelRegerarFormaPagamentoParaCobrancaPagaException>(() => _cobrancaBoletoFormaPagamentoFinalizada.RegerarFormaPagamento());
        }

        [Test]
        public void AoRegerarFormaPagamentoParaCobrancaComFormaPagamentoPendenteDeveLancarExcecao()
        {
            Assert.Throws<ImpossivelRegerarFormaQuandoFormaPagamentoPendenteException>(() => _cobrancaCartao.RegerarFormaPagamento());
        }

        [Test]
        public void AoRegerarFormaPagamentoDeveAdicionarEventoCobranca()
        {
            var formaPagamentoEvent = _cobrancaBoletoFormaPagamentoFinalizada
                .Events
                .Where(e => e is FormaPagamentoRegeradaEvent)
                .Cast<FormaPagamentoRegeradaEvent>();

            Assert.IsNull(formaPagamentoEvent.SingleOrDefault());

            var formaPagamentoAnterior = _cobrancaBoletoFormaPagamentoFinalizada.FormaPagamento;

            _cobrancaBoletoFormaPagamentoFinalizada.RegerarFormaPagamento();

            Assert.AreSame(formaPagamentoEvent.SingleOrDefault().FormaPagamentoAnterior, formaPagamentoAnterior);
            Assert.AreSame(formaPagamentoEvent.SingleOrDefault().FormaPagamento, _cobrancaBoletoFormaPagamentoFinalizada.FormaPagamento);
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

            _cobrancaBoletoFormaPagamentoFinalizada.AlterarCobranca(20, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

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
            var cobrancaCartao = _cobrancaCartaoFormaPagamentoFinalizada;
            var valor = new Random().Next(201, 2000);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(valor, cobrancaBoleto.Vencimento, cobrancaBoleto.EmissorId, cobrancaBoleto.PagadorId, cobrancaBoleto.ContaBancariaId, cobrancaBoleto);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(valor, cobrancaCartao.Vencimento, cobrancaCartao.EmissorId, cobrancaCartao.PagadorId, cobrancaCartao.ContaBancariaId, cobrancaCartao);
        }

        [Test]
        public void AoAlterarEmissorCobrancaDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartaoFormaPagamentoFinalizada;
            var emissorId = Guid.NewGuid().ToString();
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, emissorId, cobrancaBoleto.PagadorId, cobrancaBoleto.ContaBancariaId, cobrancaBoleto);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, cobrancaCartao.Vencimento, emissorId, cobrancaCartao.PagadorId, cobrancaCartao.ContaBancariaId, cobrancaCartao);
        }

        [Test]
        public void AoAlterarPagadorCobrancaDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartaoFormaPagamentoFinalizada;
            var pagadorId = Guid.NewGuid().ToString();
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, cobrancaBoleto.EmissorId, pagadorId, cobrancaBoleto.ContaBancariaId, cobrancaBoleto);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, cobrancaCartao.Vencimento, cobrancaCartao.EmissorId, pagadorId, cobrancaCartao.ContaBancariaId, cobrancaCartao);
        }

        [Test]
        public void AoAlterarContaBancariaCobrancaDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartaoFormaPagamentoFinalizada;
            var contaBancariaId = Guid.NewGuid().ToString();
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, cobrancaBoleto.EmissorId, cobrancaBoleto.PagadorId, contaBancariaId, cobrancaBoleto);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, cobrancaCartao.Vencimento, cobrancaCartao.EmissorId, cobrancaCartao.PagadorId, contaBancariaId, cobrancaCartao);
        }

        [Test]
        public void AoAlterarCobrancaSemNovosValoresNaoDeveRegerarFormaPagamento()
        {
            var cobrancaBoleto = _cobrancaBoletoFormaPagamentoFinalizada;
            var cobrancaCartao = _cobrancaCartaoFormaPagamentoFinalizada;
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaBoleto.Valor, cobrancaBoleto.Vencimento, cobrancaBoleto.EmissorId, cobrancaBoleto.PagadorId, cobrancaBoleto.ContaBancariaId, cobrancaBoleto, false);
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, cobrancaCartao.Vencimento, cobrancaCartao.EmissorId, cobrancaCartao.PagadorId, cobrancaCartao.ContaBancariaId, cobrancaCartao, false);
        }

        [Test]
        public void AoAlterarVencimentoCobrancaCartaoNaoDeveRegerarFormaPagamento()
        {
            var gen = new Random();
            var cobrancaCartao = _cobrancaCartaoFormaPagamentoFinalizada;
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            TestaRegeraFormaPagamentoNaAlteracaoCobranca(cobrancaCartao.Valor, start.AddDays(gen.Next(range)), cobrancaCartao.EmissorId, cobrancaCartao.PagadorId, 
                cobrancaCartao.ContaBancariaId, cobrancaCartao, validaRegeraFormaPagamento: false);
        }

        private void TestaRegeraFormaPagamentoNaAlteracaoCobranca(decimal valor, DateTime vencimento, string emissorId, string pagadorId, string contaBancariaId, Cobranca cobranca, bool validaRegeraFormaPagamento = true)
        {
            var formaPagamentoAnterior = cobranca.FormaPagamento;

            var formaPagamentoRegeradaEvent = cobranca.Events.Where(e => e is FormaPagamentoRegeradaEvent)
                .Cast<FormaPagamentoRegeradaEvent>();

            Assert.IsNull(formaPagamentoRegeradaEvent.SingleOrDefault());

            cobranca.AlterarCobranca(valor, vencimento, emissorId, pagadorId, contaBancariaId);

            if (validaRegeraFormaPagamento)
            {
                Assert.AreEqual(cobranca.FormaPagamento.Status, StatusFormaPagamento.Processando);
                Assert.AreSame(formaPagamentoRegeradaEvent.SingleOrDefault()?.FormaPagamentoAnterior, formaPagamentoAnterior);
                Assert.AreSame(formaPagamentoRegeradaEvent.SingleOrDefault()?.FormaPagamento, cobranca.FormaPagamento);
            }
            else
            {
                Assert.IsNull(formaPagamentoRegeradaEvent.SingleOrDefault());
                Assert.AreSame(cobranca.FormaPagamento, formaPagamentoAnterior);
                Assert.AreEqual(cobranca.FormaPagamento.Status, formaPagamentoAnterior.Status);
            }
        }
    }

    public static class CobrancaBuilder
    {
        public static Cobranca ComFormaPagamentoFinalizada(this Cobranca cobranca)
            => cobranca
                .FinalizaProcessamentoFormaPagamento(Guid.NewGuid().ToString());

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