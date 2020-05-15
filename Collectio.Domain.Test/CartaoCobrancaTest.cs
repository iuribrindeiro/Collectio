using System;
using System.Linq;
using Collectio.Domain.CobrancaCartaoAggregate;
using Collectio.Domain.CobrancaCartaoAggregate.Events;
using NUnit.Framework;

namespace Collectio.Domain.Test
{
    public class CartaoCobrancaTest
    {
        private CobrancaCartao _cobrancaCartao;

        [SetUp]
        public void Setup()
        {
            _cobrancaCartao = new CobrancaCartao(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 200, new CartaoValueObject(DateTime.Today, "202011115522", "123", "Teste Cartao Silva"));
        }


        [Test]
        public void AoCriarCobrancaCartaoDeveAdicionarEventoCobrancaCartao()
        {
            var cobrancaCartaoEvent = _cobrancaCartao.Events
                .Where(e => e is CobrancaCartaoCriadaEvent)
                .Cast<CobrancaCartaoCriadaEvent>();

            Assert.AreSame(cobrancaCartaoEvent.SingleOrDefault()?.CobrancaCartao, _cobrancaCartao);
        }

        [Test]
        public void AoCriarCobrancaCartaoTodosOsCamposDeveSerSetadosCorretamente()
        {
            var emissorId = Guid.NewGuid().ToString();
            var pagadorId = Guid.NewGuid().ToString();
            var valor = 200;
            var vencimentoCartao = DateTime.Today;
            var numero = "123456789";
            var codigoSeguranca = "123";
            var nome = "Teste Cartao bla";

            var cartaoValueObject = new CartaoValueObject(vencimentoCartao, numero, codigoSeguranca, nome);

            var cobrancaCartao = new CobrancaCartao(emissorId, pagadorId, valor, cartaoValueObject);

            Assert.AreEqual(cobrancaCartao.EmissorId, emissorId);
            Assert.AreEqual(cobrancaCartao.PagadorId, pagadorId);
            Assert.AreEqual(cobrancaCartao.Valor, valor);
            Assert.AreSame(cobrancaCartao.Cartao, cartaoValueObject);

            Assert.AreEqual(vencimentoCartao, cobrancaCartao.Cartao.Vencimento);
            Assert.AreEqual(numero, cobrancaCartao.Cartao.Numero);
            Assert.AreEqual(codigoSeguranca, cobrancaCartao.Cartao.CodigoSeguranca);
            Assert.AreEqual(nome, cobrancaCartao.Cartao.Nome);
        }
    }
}
