﻿using System;
using System.Linq;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using Collectio.Domain.ConfiguracaoEmissaoAggregate.Events;
using Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions;
using NUnit.Framework;

namespace Collectio.Domain.Test
{
    public class ConfiguracaoEmissaoTest
    {
        [Test]
        public void AoCriarConfiguracaoEmissaoDeveSetarValoresCorretamente()
        {
            var nomeEmpresa = "Teste";
            var cpfCnpj = "12344";
            var agencia = "1234222";
            var conta = "9988";
            var telefone = "242424";
            var ddd = "55";
            var email = "asdasdsad@email.com";

            var configuracaoEmissao = ConfiguracaoEmissaoBuilder.Build(nomeEmpresa, cpfCnpj, agencia, conta, telefone, ddd, email);

            Assert.AreEqual(configuracaoEmissao.NomeEmpresa, nomeEmpresa);
            Assert.AreEqual(configuracaoEmissao.CpfCnpj.Value, cpfCnpj);
            Assert.AreEqual(configuracaoEmissao.AgenciaConta.Agencia, agencia);
            Assert.AreEqual(configuracaoEmissao.AgenciaConta.Conta, conta);
            Assert.AreEqual(configuracaoEmissao.Telefone.Telefone, telefone);
            Assert.AreEqual(configuracaoEmissao.Telefone.Ddd, ddd);
            Assert.AreEqual(configuracaoEmissao.Email.Value, email);
        }
        
        [Test]
        public void AoCriarConfiguracaoEmissaoDeveSetarStatusComoProcessando()
        {
            var configuracaoEmissao = ConfiguracaoEmissaoBuilder.Build();

            Assert.AreEqual(configuracaoEmissao.Status.Status, StatusConfiguracaoEmissao.Processando);
        }

        [Test]
        public void AoCriarConfiguracaoEmissaoDeveAdicionarEventoConfiguracaoEmissao()
        {
            var configuracaoEmissao = ConfiguracaoEmissaoBuilder.Build();
            var cobrancaCriadaEvent = configuracaoEmissao.Events.Cast<ConfiguracaoEmissaoCriadaEvent>().First();

            Assert.AreEqual(cobrancaCriadaEvent.ConfiguracaoEmissaoId, configuracaoEmissao.Id.ToString());
        }

        [Test]
        public void AoAlterarConfiguracaoEmissaoDeveSetarDadosCorretamente()
        {
            var nomeEmpresa = "Teste";
            var cpfCnpj = "12344";
            var agencia = "1234222";
            var conta = "9988";
            var telefone = "242424";
            var ddd = "55";
            var email = "asdasdsad@email.com";

            var configuracaoEmissao = ConfiguracaoEmissaoBuilder.Build().ComStatus(StatusConfiguracaoEmissao.Processado);

            configuracaoEmissao.Alterar(nomeEmpresa, agencia, conta, cpfCnpj, email, telefone, ddd);

            Assert.AreEqual(configuracaoEmissao.NomeEmpresa, nomeEmpresa);
            Assert.AreEqual(configuracaoEmissao.CpfCnpj.Value, cpfCnpj);
            Assert.AreEqual(configuracaoEmissao.AgenciaConta.Agencia, agencia);
            Assert.AreEqual(configuracaoEmissao.AgenciaConta.Conta, conta);
            Assert.AreEqual(configuracaoEmissao.Telefone.Telefone, telefone);
            Assert.AreEqual(configuracaoEmissao.Telefone.Ddd, ddd);
            Assert.AreEqual(configuracaoEmissao.Email.Value, email);
        }

        [Test]
        public void AoAlterarConfiguracaoEmissaoProcessandoDeveLancarExcecao()
        {
            var configuracaoEmissao = ConfiguracaoEmissaoBuilder.Build();

            Assert.Throws<ImpossivelAlterarConfiguracaoEmissaoEmProcessamentoException>(() => 
                configuracaoEmissao.Alterar(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 
                    Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }

        [Test, Sequential]
        public void AoAlterarConfiguracaoEmissaoComNovosDadosDeveReprocessarConfiguracaoEmissao(
            [Values("Nome Teste 1", "Nome Teste", "Nome Teste", "Nome Teste", "Nome Teste", "Nome Teste", "Nome Teste")] string nomeEmpresa,  
            [Values("AG1", "AG2", "AG1", "AG1", "AG1", "AG1", "AG1")] string agencia, 
            [Values("C2", "C2", "C3", "C2", "C2", "C2", "C2")] string conta, 
            [Values("CPF123", "CPF123", "CPF123", "CPF1234", "CPF123", "CPF123")] string cpfCnpj, 
            [Values("Email1", "Email1", "Email1", "Email1", "Email12", "Email1", "Email1")] string email, 
            [Values("telefone1", "telefone1", "telefone1", "telefone1", "telefone1", "telefone12", "telefone1")] string telefone,
            [Values("ddd1", "ddd1", "ddd1", "ddd1", "ddd1", "ddd1", "ddd2")] string ddd)
        {
            var configuracaoEmissao = ConfiguracaoEmissaoBuilder.Build().ComStatus(StatusConfiguracaoEmissao.Processado);

            configuracaoEmissao.Alterar(nomeEmpresa, agencia, conta, cpfCnpj, email, telefone, ddd);

            Assert.IsNotNull(configuracaoEmissao.Events.Where(e => e is ConfiguracaoEmissaoReprocessandoEvent).Cast<ConfiguracaoEmissaoReprocessandoEvent>().SingleOrDefault());
        }

        [Test]
        public void AoAlterarConfiguracaoEmissaoDeveAdicionarEventoConfiguracaoEmissao()
        {
            var configuracaoEmissao = ConfiguracaoEmissaoBuilder.Build().ComStatus(StatusConfiguracaoEmissao.Processado);

            var agenciaAnterior = configuracaoEmissao.AgenciaConta.Agencia;
            var contaAnterior = configuracaoEmissao.AgenciaConta.Conta;
            var cpfCnpjAnterior = configuracaoEmissao.CpfCnpj.Value;
            var emailAnterior = configuracaoEmissao.Email.Value;
            var telefoneAnterior = configuracaoEmissao.Telefone.Telefone;
            var dddAnterior = configuracaoEmissao.Telefone.Ddd;

            var configuracaoEmissaoAlteradaEvent = configuracaoEmissao.Events
                .Where(e => e is ConfiguracaoEmissaoAlteradaEvent)
                .Cast<ConfiguracaoEmissaoAlteradaEvent>();

            Assert.IsNull(configuracaoEmissaoAlteradaEvent.SingleOrDefault());

            configuracaoEmissao.Alterar("Teste", "ag", "conta", "cpf", "email", "telefone", "ddd");


            var @event = configuracaoEmissaoAlteradaEvent.SingleOrDefault();

            Assert.IsNotNull(@event);

            Assert.AreEqual(@event?.AgenciaAnterior, agenciaAnterior);
            Assert.AreEqual(@event?.ContaAnterior, contaAnterior);
            Assert.AreEqual(@event?.CpfCnpjAnterior, cpfCnpjAnterior);
            Assert.AreEqual(@event?.EmailAnterior, emailAnterior);
            Assert.AreEqual(@event?.TelefoneAnterior, telefoneAnterior);
            Assert.AreEqual(@event?.DddAnterior, dddAnterior);
        }

        [Test]
        public void AoFinalizarProcessamentoConfiguracaoEmissaoDeveSetarValoresCorretamente()
        {
            var configuracaoEmissao = ConfiguracaoEmissaoBuilder.Build();
            configuracaoEmissao.FinalizarProcessamento();

            Assert.AreEqual(configuracaoEmissao.Status.Status, StatusConfiguracaoEmissao.Processado);
        }

        public void AoProcessarConfiguracaoEmissaoJaProcessadaDeveLancarExcecao()
        {

        }

        public void AoProcessarConfiguracaoEmissaoDeveAdicionarEventoConfiguracaoEmissao()
        {

        }

        public void AoDefinirConfiguracaoEmissaoErroDeveSetarValoresCorretamente()
        {

        }

        public void AoDefinirConfiguracaoEmissaoErroQuandoStatusDifereDeProcessandoDeveLancarExcecao()
        {

        }

        public void AoDefinirConfiguracaoEmissaoComErroDeveAdicionarEventoConfiguracaoEmissao()
        {

        }

        public void AoReprocessarConfiguracaoEmissaoDeveSetarValoresCorretamente()
        {

        }

        public void AoReprocessarConfiguracaoEmissaoComStatusProcessandoDeveLancarExcecao()
        {

        }

        public void AoReprocessarConfiguracaoEmissaoDeveAdicionarEventoConfiguracaoEmissao()
        {

        }
    }

    public static class ConfiguracaoEmissaoBuilder
    {
        public static ConfiguracaoEmissao Build(string nomeEmpresa, string cpfCnpj, string agencia, string conta, string telefone, string ddd, string email) 
            => new ConfiguracaoEmissao(nomeEmpresa, agencia, conta, cpfCnpj, email, telefone, ddd);

        public static ConfiguracaoEmissao Build() =>
            new ConfiguracaoEmissao("Nome Teste", "AG1", "C2", "CPF123", "Email1", "telefone1", "ddd1");

        public static ConfiguracaoEmissao ComStatus(this ConfiguracaoEmissao configuracaoEmissao,
            StatusConfiguracaoEmissao status)
        {
            if (status == StatusConfiguracaoEmissao.Processado)
                return configuracaoEmissao.FinalizarProcessamento();
            else if (status == StatusConfiguracaoEmissao.Erro)
                return configuracaoEmissao.ErroProcessamento("Falha");

            return Build();
        }
    }
}
