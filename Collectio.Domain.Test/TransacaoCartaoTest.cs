using System;
using System.Linq;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.TransacaoCartaoAggregate;
using Collectio.Domain.TransacaoCartaoAggregate.Events;
using NUnit.Framework;

namespace Collectio.Domain.Test
{
    public class TransacaoCartaoTest
    {
        private TransacaoCartao _transacaoCartao;
        private CartaoValueObject _dadosCartao;
        private CpfCnpjValueObject _cpfCnpjProprietario;

        [SetUp]
        public void Setup()
        {
            _cpfCnpjProprietario = new CpfCnpjValueObject("12345678925");
            _dadosCartao = new CartaoValueObject(DateTime.Today, "202011115522", "123", "Teste Cartao Silva", _cpfCnpjProprietario);
            _transacaoCartao = new TransacaoCartao(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 200, _dadosCartao);
        }


        [Test]
        public void AoCriarTransacaoCartaoDeveAdicionarEventoTransacaoCartao()
        {
            var transacaoCartaoEvent = _transacaoCartao.Events
                .Where(e => e is TransacaoCartaoCriadaEvent)
                .Cast<TransacaoCartaoCriadaEvent>();

            Assert.AreSame(transacaoCartaoEvent.SingleOrDefault()?.TransacaoCartao, _transacaoCartao);
        }

        [Test]
        public void AoCriarTransacaoCartaoTodosOsCamposDeveSerSetadosCorretamente()
        {
            var idCobranca = Guid.NewGuid().ToString();
            var emissorId = Guid.NewGuid().ToString();
            var pagadorId = Guid.NewGuid().ToString();
            var valor = 200;
            var vencimentoCartao = DateTime.Today;
            var numero = "123456789";
            var codigoSeguranca = "123";
            var nome = "Teste Cartao bla";

            var cartaoValueObject = new CartaoValueObject(vencimentoCartao, numero, codigoSeguranca, nome, new CpfCnpjValueObject("12345678925"));

            var transacaoCartao = new TransacaoCartao(idCobranca, emissorId, pagadorId, valor, cartaoValueObject);

            Assert.AreEqual(transacaoCartao.IdCobranca, idCobranca);
            Assert.AreEqual(transacaoCartao.EmissorId, emissorId);
            Assert.AreEqual(transacaoCartao.PagadorId, pagadorId);
            Assert.AreEqual(transacaoCartao.Valor, valor);
            Assert.AreSame(transacaoCartao.Cartao, cartaoValueObject);

            Assert.AreEqual(vencimentoCartao, transacaoCartao.Cartao.Vencimento);
            Assert.AreEqual(numero, transacaoCartao.Cartao.Numero);
            Assert.AreEqual(codigoSeguranca, transacaoCartao.Cartao.CodigoSeguranca);
            Assert.AreEqual(nome, transacaoCartao.Cartao.Nome);
        }

        [Test]
        public void AoTentarAlterarTransacaoCartaoParaUmStatusInvalidoDeveLancarExcecao([Values(
            StatusTransacaoCartao.Erro, StatusTransacaoCartao.Aprovada)] StatusTransacaoCartao statusAtual)
        {
            if (statusAtual == StatusTransacaoCartao.Erro)
            {
                _transacaoCartao.ErroTransacao("Sem limite disponivel");
                Assert.Throws<Exception>(() => _transacaoCartao.AprovarTransacao(Guid.NewGuid().ToString()));
                Assert.Throws<Exception>(() => _transacaoCartao.ErroTransacao("Falha"));
            }
            else if (statusAtual == StatusTransacaoCartao.Aprovada)
            {
                _transacaoCartao.AprovarTransacao(Guid.NewGuid().ToString());
                Assert.Throws<Exception>(() => _transacaoCartao.ErroTransacao("Sem limite disponivel"));
                Assert.Throws<Exception>(() => _transacaoCartao.AprovarTransacao(Guid.NewGuid().ToString()));
            }
        }

        [Test]
        public void ConsigoMudarStatusTransacaoParaErroOuAprovada()
        {
            var novaTransacao = new TransacaoCartao(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 200, _dadosCartao);
            novaTransacao.ErroTransacao("Falha ao realizar pagamento");

            _transacaoCartao.AprovarTransacao(Guid.NewGuid().ToString());
            Assert.AreEqual(_transacaoCartao.Status.Status, StatusTransacaoCartao.Aprovada);
            Assert.AreEqual(novaTransacao.Status.Status, StatusTransacaoCartao.Erro);
        }

        [Test]
        public void AoFinalizarTransacaoDeveSetarOIdTransacaoCorretamente()
        {
            var idTransacao = Guid.NewGuid().ToString();
            _transacaoCartao.AprovarTransacao(idTransacao);

            Assert.AreEqual(_transacaoCartao.Status.TransacaoId, idTransacao);
        }

        [Test]
        public void AoFinalizarTransacaoCartaoDeveAdicionarEventoTransacaoCartao()
        {
            var transacaoCartao = _transacaoCartao
                .Events
                .Where(e => e is TransacaoCartaoAprovadaEvent)
                .Cast<TransacaoCartaoAprovadaEvent>();

            Assert.IsNull(transacaoCartao.SingleOrDefault());

            _transacaoCartao.AprovarTransacao(Guid.NewGuid().ToString());

            Assert.AreSame(transacaoCartao.SingleOrDefault().TransacaoCartao, _transacaoCartao);
        }

        [Test]
        public void AoDefinirTransacaoCartaoComoErroDeveAdicionarEventoTransacaoCartao()
        {
            var transacaoCartao = _transacaoCartao
                .Events
                .Where(e => e is ErroTransacaoCartaoEvent)
                .Cast<ErroTransacaoCartaoEvent>();

            Assert.IsNull(transacaoCartao.SingleOrDefault());

            _transacaoCartao.ErroTransacao("Sem limite");

            Assert.AreSame(transacaoCartao.SingleOrDefault().TransacaoCartao, _transacaoCartao);
        }
    }
}
