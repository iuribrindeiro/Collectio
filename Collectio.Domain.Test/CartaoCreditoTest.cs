﻿using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using Collectio.Domain.ClienteAggregate.Exceptions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Collectio.Domain.Test
{
    public class CartaoCreditoTest
    {

        [Test]
        public void AoAdicionarCartaoCreditoDeveAdicionarEventoCartaoCredito()
        {
            var dadosCartao = CartaoCreditoBuilder.BuildDadosCartao();
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito(dadosCartao);
            var cartaoCreditoEvent = cartaoCredito
                .Events
                .Where(e => e is CartaoCreditoCriadoEvent)
                .Cast<CartaoCreditoCriadoEvent>();

            Assert.AreEqual(cartaoCreditoEvent.SingleOrDefault()?.CartaoId, cartaoCredito.Id.ToString());
            Assert.AreSame(cartaoCreditoEvent.SingleOrDefault()?.DadosCartao, dadosCartao);
        }

        [Test]
        public void AoProcessarCartaoDeveAdicionarEventoACartaoCredito()
        {
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito();
            var numero = Guid.NewGuid().ToString();
            cartaoCredito.Processado(numero);
            var cartaoCreditoEvent = cartaoCredito
                .Events
                .Where(e => e is CartaoCreditoProcessadoEvent)
                .Cast<CartaoCreditoProcessadoEvent>();

            Assert.AreEqual(cartaoCreditoEvent.SingleOrDefault()?.Numero, numero);
            Assert.AreEqual(cartaoCreditoEvent.SingleOrDefault()?.CartaoId, cartaoCredito.Id.ToString());
        }

        [Test]
        public void AoDefinirErroCartaoDeveAdicionarEventoACartaoCredito()
        {
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito();
            var mensagemErro = Guid.NewGuid().ToString();
            cartaoCredito.Erro(mensagemErro);
            var cartaoCreditoEvent = cartaoCredito
                .Events
                .Where(e => e is ErroProcessarCartaoCreditoEvent)
                .Cast<ErroProcessarCartaoCreditoEvent>();

            Assert.AreEqual(cartaoCreditoEvent.SingleOrDefault()?.MensagemErro, mensagemErro);
            Assert.AreEqual(cartaoCreditoEvent.SingleOrDefault()?.CartaoId, cartaoCredito.Id.ToString());
        }

        [Test]
        public void AoCriarCartaoCreditoDeveSetarDadosCorretamente()
        {
            var cpfCnpjProprietario = "12345678912";
            var clienteId = Guid.NewGuid().ToString();
            var numero = "1234";
            var codigo = "12345";
            var nome = "bla";
            var vencimento = DateTime.Today.AddDays(22);
            var dadosCartao = CartaoCreditoBuilder.BuildDadosCartao(numero, codigo, nome, vencimento);
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito(cpfCnpjProprietario, clienteId, dadosCartao);

            Assert.AreEqual(cartaoCredito.TenantId, clienteId);
            Assert.AreEqual(cartaoCredito.CpfCnpjProprietario, cpfCnpjProprietario);
            Assert.AreEqual(numero, dadosCartao.Numero);
            Assert.AreEqual(codigo, dadosCartao.CodigoSeguranca);
            Assert.AreEqual(nome, dadosCartao.NomeProprietario);
            Assert.AreEqual(vencimento, dadosCartao.Vencimento);
        }


        [Test]
        public void AoAdicionarTransacaoCartaoNaoProcessadoDeveLancarExcecao()
        {
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito();
            var cartaoCreditoComErro = CartaoCreditoBuilder.BuildCartaoCredito().ComStatus(StatusCartao.Erro);
            Assert.Throws<CartaoCreditoNaoProcessadoException>(() => cartaoCredito.AddTransacao(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 22));
            Assert.Throws<CartaoCreditoNaoProcessadoException>(() => cartaoCreditoComErro.AddTransacao(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 22));
        }

        [Test]
        public void AoAdicionarTransacaoCobrancaDeveAdicionarCorretamenteALista()
        {
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito().ComStatus(StatusCartao.Processado);
            var cobId = Guid.NewGuid().ToString();
            var contaBancaria = Guid.NewGuid().ToString();
            var valorCobranca = 22;
            cartaoCredito.AddTransacao(cobId, contaBancaria, 22);
            var transacao = cartaoCredito.Transacoes.SingleOrDefault();
            Assert.IsNotNull(transacao);
            Assert.AreEqual(transacao.CobrancaId, cobId);
            Assert.AreEqual(transacao.Valor, valorCobranca);
        }

        [Test]
        public void AoProcessarCartaoQuandoStatusForDiferenteProcessandoDeveLancarExcecao()
        {
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito().ComStatus(StatusCartao.Processado);
            var cartaoCreditoComErro = CartaoCreditoBuilder.BuildCartaoCredito().ComStatus(StatusCartao.Erro);
            Assert.Throws<ImpossivelDefinirStatusCartaoComoProcessadoException>(() => cartaoCredito.Processado("123"));
            Assert.Throws<ImpossivelDefinirStatusCartaoComoProcessadoException>(() => cartaoCreditoComErro.Processado("1234"));
        }

        [Test]
        public void AoDefinirErroCartaoQuandoStatusForDiferenteProcessandoDeveLancarExcecao()
        {
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito().ComStatus(StatusCartao.Processado);
            var cartaoCreditoComErro = CartaoCreditoBuilder.BuildCartaoCredito().ComStatus(StatusCartao.Erro);
            Assert.Throws<ImpossivelDefinirStatusCartaoComoErroException>(() => cartaoCredito.Erro(Guid.NewGuid().ToString()));
            Assert.Throws<ImpossivelDefinirStatusCartaoComoErroException>(() => cartaoCreditoComErro.Erro(Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoProcessarCartaoStatusDeveSerProcessadoComNumeroEToken()
        {
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito();
            var numero = Guid.NewGuid().ToString();

            cartaoCredito.Processado(numero);

            Assert.AreEqual(cartaoCredito.Status.Status, StatusCartao.Processado);
            Assert.AreEqual(cartaoCredito.Numero, numero);
        }

        [Test]
        public void AoDefinirErroCartaoStatusDeveSerErroComMensagemErro()
        {
            var cartaoCredito = CartaoCreditoBuilder.BuildCartaoCredito();
            var mensagemErro = Guid.NewGuid().ToString();

            cartaoCredito.Erro(mensagemErro);

            Assert.AreEqual(cartaoCredito.Status.Status, StatusCartao.Erro);
            Assert.AreEqual(cartaoCredito.Status.MensagemErro, mensagemErro);
        }
    }

    public static class CartaoCreditoBuilder
    {
        public static CartaoCredito BuildCartaoCredito()
            => new CartaoCredito("12345678921", Guid.NewGuid().ToString(), BuildDadosCartao());

        public static CartaoCredito BuildCartaoCredito(DadosCartaoValueObject dadosCartao)
            => new CartaoCredito("12345678921", Guid.NewGuid().ToString(), dadosCartao);

        public static CartaoCredito BuildCartaoCredito(string cpfCnpj, string clienteId, DadosCartaoValueObject dadosCartaoValue)
            => new CartaoCredito(cpfCnpj, clienteId, dadosCartaoValue);

        public static DadosCartaoValueObject BuildDadosCartao() 
            => new DadosCartaoValueObject("1234", "1234", "Teste name", DateTime.Today);

        public static DadosCartaoValueObject BuildDadosCartao(string numero, string codigoSeguranca, string nome, DateTime vencimento)
            => new DadosCartaoValueObject(numero, codigoSeguranca, nome, vencimento);

        public static CartaoCredito ComStatus(this CartaoCredito cartaoCredito, StatusCartao status)
        {
            if (status == StatusCartao.Erro)
                return cartaoCredito.Erro("Falha");
            else if (status == StatusCartao.Processado)
                return cartaoCredito.Processado("1234");

            return cartaoCredito;
        }
    }
}

