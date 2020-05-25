using System;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.Exceptions;
using NUnit.Framework;

namespace Collectio.Domain.Test
{
    public class BaseValueObjectsTests
    {
        [Test]
        public void AoCriarTelefoneDeveSetarValoresCorretamente()
        {
            var ddd = Guid.NewGuid().ToString();
            var numero = Guid.NewGuid().ToString();

            var telefone = new Telefone(ddd, numero);

            Assert.AreEqual(ddd, telefone.Ddd);
            Assert.AreEqual(numero, telefone.Numero);
        }

        [Test]
        public void AoCriarEnderecoDeveSetarValoresCorretamente()
        {
            string cidade = Guid.NewGuid().ToString();
            string rua = Guid.NewGuid().ToString();
            string numero = Guid.NewGuid().ToString();
            string bairro = Guid.NewGuid().ToString();
            string cep = Guid.NewGuid().ToString();
            string uf = Guid.NewGuid().ToString();

            var enderedo = new Endereco(rua, numero, bairro, cep, uf, cidade);

            Assert.AreEqual(enderedo.Rua, rua);
            Assert.AreEqual(enderedo.Numero, numero);
            Assert.AreEqual(enderedo.Bairro, bairro);
            Assert.AreEqual(enderedo.Cep, cep);
            Assert.AreEqual(enderedo.Uf, uf);
            Assert.AreEqual(enderedo.Cidade, cidade);
        }
    }


    public static class ClienteCobrancaBuilder
    {
        public static Telefone BuildTelefone() 
            => new Telefone("dd", "12314");

        public static Endereco BuildEndereco() 
            => new Endereco("aa", "12331", "bairro", "1232313", "DE", "bla");

        public static CartaoCredito BuildCartaoCredito() 
            => new CartaoCredito("Bla bla", "12314", Guid.NewGuid().ToString());

        public static ClienteCobranca BuildCliente(Cobranca cobranca)
        {
            var cartaoCredito = cobranca.FormaPagamentoCartao ? ClienteCobrancaBuilder.BuildCartaoCredito() : null;

            return new ClienteCobranca(cobranca, "Bla bla", "cpf", "emailbla@email", Guid.NewGuid().ToString(),
                BuildTelefone(), BuildEndereco(), cartaoCredito);
        }

        public static ClienteCobranca BuildCliente(FormaPagamento formaPagamentoCobranca)
        {
            var cobranca = formaPagamentoCobranca == FormaPagamento.Boleto
                ? CobrancaBuilder.BuildCobrancaBoleto()
                : CobrancaBuilder.BuildCobrancaCartao();
            var cartaoCredito = cobranca.FormaPagamentoCartao ? ClienteCobrancaBuilder.BuildCartaoCredito() : null;

            return new ClienteCobranca(cobranca, "Bla bla", "cpf", "emailbla@email", Guid.NewGuid().ToString(),
                BuildTelefone(), BuildEndereco(), cartaoCredito);
        }
    }

    public class ClienteCobrancaTest
    {

        [Test]
        public void AoCriarCartaoCreditoDeveSetarDadosCorretamente()
        {
            var tenantId = Guid.NewGuid().ToString();
            var nome = Guid.NewGuid().ToString();
            var numero = Guid.NewGuid().ToString();

            var cartaoCredito = new CartaoCredito(nome, numero, tenantId);

            Assert.AreEqual(cartaoCredito.Numero, numero);
            Assert.AreEqual(cartaoCredito.TenantId, tenantId);
            Assert.AreEqual(cartaoCredito.Nome, nome);
        }

        [Test]
        public void AoCriarClienteSemCartaoCreditoECobrancaDeCartaoDeveLancarExcecao()
        {
            var cobrancaCartao = CobrancaBuilder.BuildCobrancaCartao();
            Assert.Throws<CobrancasComCartaoDevemPossuirClienteComCartaoCreditoVinculadoException>(()
                => new ClienteCobranca(cobrancaCartao, "Bla bla", "cpf", "emailbla@email", Guid.NewGuid().ToString(), 
                    ClienteCobrancaBuilder.BuildTelefone(), ClienteCobrancaBuilder.BuildEndereco(), null));
        }

        [Test]
        public void AoCriarClienteComCartaoCreditoECobrancaNaoCartaoDeveLancarExcecao()
        {
            var cobrancaCartao = CobrancaBuilder.BuildCobrancaBoleto();
            Assert.Throws<CobrancaBoletoNaoDeveConterCartaoNoClienteException>(()
                => new ClienteCobranca(cobrancaCartao, "Bla bla", "cpf", "emailbla@email", Guid.NewGuid().ToString(),
                    ClienteCobrancaBuilder.BuildTelefone(), ClienteCobrancaBuilder.BuildEndereco(), ClienteCobrancaBuilder.BuildCartaoCredito()));
        }

        [Test]
        public void AoCriarClienteDeveSetarValoresCorretamente()
        {
            var cobrancaCartao = CobrancaBuilder.BuildCobrancaCartao();
            var telefone = ClienteCobrancaBuilder.BuildTelefone();
            var endereco = ClienteCobrancaBuilder.BuildEndereco();
            var nome = Guid.NewGuid().ToString();
            var cpfCnpj = Guid.NewGuid().ToString();
            var email = Guid.NewGuid().ToString();
            var tenantId = Guid.NewGuid().ToString();
            var cartaoCredito = ClienteCobrancaBuilder.BuildCartaoCredito();

            var cliente = new ClienteCobranca(cobrancaCartao, nome, cpfCnpj, email, tenantId, telefone, endereco, cartaoCredito);


            Assert.AreEqual(cliente.Nome, nome);
            Assert.AreEqual(cliente.CpfCnpj, cpfCnpj);
            Assert.AreEqual(cliente.Email, email);
            Assert.AreEqual(cliente.TenantId, tenantId);
            Assert.AreSame(cliente.Telefone, telefone);
            Assert.AreSame(cliente.Endereco, endereco);
            Assert.AreEqual(cliente.CartaoCredito.TenantId, cartaoCredito.TenantId);
            Assert.AreEqual(cliente.CartaoCredito.Nome, cartaoCredito.Nome);
            Assert.AreEqual(cliente.CartaoCredito.Numero, cartaoCredito.Numero);
        }

        [Test]
        public void AoAlterarCartaoCreditoComCobrancaProcessandoDeveLancarExcecao()
        {
            var cliente = ClienteCobrancaBuilder.BuildCliente(FormaPagamento.Cartao);

            Assert.Throws<ImpossivelAlterarDadosClienteQuandoCobrancaEstaEmProcessamentoException>(() 
                => cliente.AlterarCartaoCredito(new CartaoCredito("bla bla", "123", Guid.NewGuid().ToString())));
        }

        [Test]
        public void AoAlterarCartaoClienteComCobrancaPagaDeveLancarExcecao()
        {
            var cobranca = CobrancaBuilder.BuildCobrancaCartao().ComTransacaoFinalizada();

            var cliente = ClienteCobrancaBuilder.BuildCliente(cobranca);

            Assert.Throws<ImpossivelAlterarDadosClienteQuandoCobrancaJaEstaPagaException>(() => cliente.AlterarCartaoCredito(new CartaoCredito("bla", "bla", "bla")));
        }

        [Test]
        public void AoAlterarCartaoClienteComCobrancaBoletoDeveLancarExcecao()
        {
            var cobranca = CobrancaBuilder.BuildCobrancaBoleto().ComTransacaoFinalizada();

            var cliente = ClienteCobrancaBuilder.BuildCliente(cobranca);

            Assert.Throws<CobrancaBoletoNaoDeveConterCartaoNoClienteException>(() => cliente.AlterarCartaoCredito(new CartaoCredito("bla", "bla", "bla")));
        }

        [Test]
        public void AoAlterarCartaoClienteDeveSetarCartaoCorretamente()
        {
            var cobranca = CobrancaBuilder.BuildCobrancaCartao().ComErroTransacao();

            var cliente = ClienteCobrancaBuilder.BuildCliente(cobranca);

            var novoCartaoCredito = ClienteCobrancaBuilder.BuildCartaoCredito();

            cliente.AlterarCartaoCredito(novoCartaoCredito);
            Assert.AreSame(cliente.CartaoCredito, novoCartaoCredito);
        }

        [Test]
        public void AoAlterarClienteComCobrancaPagaDeveLancarExcecao()
        {
            var cobranca = CobrancaBuilder.BuildCobrancaCartao().ComTransacaoFinalizada();
            var cliente = ClienteCobrancaBuilder.BuildCliente(cobranca);

            Assert.Throws<ImpossivelAlterarDadosClienteQuandoCobrancaJaEstaPagaException>(() => cliente.Alterar(Guid.NewGuid().ToString(), "teste", "1234", "emaikl@amil.com",
                ClienteCobrancaBuilder.BuildTelefone(), ClienteCobrancaBuilder.BuildEndereco()));
        }

        [Test]
        public void AoAlterarClienteComCobrancaProcessandoDeveLancarExcecao()
        {
            var cobranca = CobrancaBuilder.BuildCobrancaCartao();
            var cliente = ClienteCobrancaBuilder.BuildCliente(cobranca);

            Assert.Throws<ImpossivelAlterarDadosClienteQuandoCobrancaEstaEmProcessamentoException>(() => cliente.Alterar(Guid.NewGuid().ToString(), "teste", "1234", "emaikl@amil.com",
                ClienteCobrancaBuilder.BuildTelefone(), ClienteCobrancaBuilder.BuildEndereco()));
        }

        [Test]
        public void AoAlterarClienteDeveSetarDadosCorretamente()
        {
            var cobranca = CobrancaBuilder.BuildCobrancaCartao().ComErroTransacao();
            var cliente = ClienteCobrancaBuilder.BuildCliente(cobranca);

            var tenantId = Guid.NewGuid().ToString();
            var nome = Guid.NewGuid().ToString();
            var email = Guid.NewGuid().ToString();
            var cpfCnpj = Guid.NewGuid().ToString();
            var telefone = ClienteCobrancaBuilder.BuildTelefone();
            var endereco = ClienteCobrancaBuilder.BuildEndereco();

            cliente.Alterar(tenantId, nome, cpfCnpj, email, telefone, endereco);

            Assert.AreEqual(tenantId, cliente.TenantId);
            Assert.AreEqual(nome, cliente.Nome);
            Assert.AreEqual(cpfCnpj, cliente.CpfCnpj);
            Assert.AreEqual(email, cliente.Email);
            Assert.AreSame(telefone, cliente.Telefone);
            Assert.AreSame(endereco, cliente.Endereco);
        }
    }
}
