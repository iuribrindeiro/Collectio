using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ClienteAggregate;
using Collectio.Domain.ClienteAggregate.Events;
using NUnit.Framework;
using System;
using System.Linq;

namespace Collectio.Domain.Test
{
    public class ClienteTest
    {
        [Test]
        public void AoCriarClienteDeveSetarDadosCorretamente()
        {
            var nome = "Iuri";
            var cpfCnpj = new CpfCnpjValueObject("12345678912");

            var cliente = new Cliente(nome, cpfCnpj);

            Assert.AreEqual(nome, cliente.Nome);
            Assert.AreEqual(cpfCnpj, cliente.CpfCnpj);
        }

        [Test]
        public void AoCriarClienteDeveAdicionarEventoCliente()
        {
            var cliente = new Cliente("Iuri", new CpfCnpjValueObject("12345678912"));
            var eventos = cliente.Events
                .Where(e => e is ClienteCriadoEvent)
                .Cast<ClienteCriadoEvent>();

            Assert.AreEqual(eventos.SingleOrDefault()?.ClienteId, cliente.Id.ToString());
        }

        [Test]
        public void AoDefinirCartaoCreditoClientePadraoDeveSetarCartaoPadraoCorretamente()
        {
            var cliente = new Cliente("Iuri", new CpfCnpjValueObject("12345678912"));
            var cartaoId = Guid.NewGuid().ToString();
            cliente.DefinirCartaoCreditoPadrao(cartaoId);
            Assert.AreEqual(cliente.CartaoCreditoPadraoId, cartaoId);
        }

        [Test]
        public void AoDefinirCartaoCreditoClientePadraoDeveAdicionarEventoCliente()
        {
            var cliente = new Cliente("Iuri", new CpfCnpjValueObject("12345678912"));
            var eventos = cliente.Events
                .Where(e => e is CartaoCreditoPadraoDefinidoEvent)
                .Cast<CartaoCreditoPadraoDefinidoEvent>();

            Assert.IsNull(eventos.SingleOrDefault());

            cliente.DefinirCartaoCreditoPadrao(Guid.NewGuid().ToString());
            var cartaoPadraoAnterior = cliente.CartaoCreditoPadraoId;

            Assert.AreEqual(eventos.SingleOrDefault().CartaoCreditoPadraoId, cliente.CartaoCreditoPadraoId);
            cliente.DefinirCartaoCreditoPadrao(Guid.NewGuid().ToString());

            Assert.AreEqual(eventos.LastOrDefault().CartaoCreditoPadraoAnteiorId, cartaoPadraoAnterior);
            Assert.AreEqual(eventos.LastOrDefault().CartaoCreditoPadraoId, cliente.CartaoCreditoPadraoId);
        }
    }
}
