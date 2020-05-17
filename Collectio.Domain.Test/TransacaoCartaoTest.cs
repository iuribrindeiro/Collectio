using Collectio.Domain.ClienteAggregate.CartaoCreditoModels;
using Collectio.Domain.ClienteAggregate.Events;
using Collectio.Domain.ClienteAggregate.Exceptions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Collectio.Domain.Test
{
    public class TransacaoCartaoTest
    {

#region SETAR DADOS CORRETAMENTE
        [Test]
        public void AoCriarTransacaoCartaoTodosOsCamposDeveSerSetadosCorretamente()
        {
            var idCobranca = Guid.NewGuid().ToString();
            var idCartao = Guid.NewGuid();
            var valor = 200;

            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao(idCobranca, idCartao, valor);

            Assert.AreEqual(transacaoCartao.CobrancaId, idCobranca);
            Assert.AreEqual(transacaoCartao.Valor, valor);
        }

        [Test]
        public void AoAprovarTransacaoDeveSetarOIdTransacaoCorretamente()
        {
            var idTransacao = Guid.NewGuid().ToString();
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Procesando);
            transacao.Aprovar(idTransacao);
            Assert.AreEqual(transacao.ExternalTenantId, idTransacao);
        }

        [Test]
        public void AoDefinirTransacaoComoErroDeveSetarIdTransacaoEMensagemCorretamente()
        {
            var mensagemErro = "Sem limite";
            var idTransacao = Guid.NewGuid().ToString();
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Procesando);
            transacao.DefinirErro(mensagemErro, idTransacao);

            Assert.AreEqual(transacao.ExternalTenantId, idTransacao);
            Assert.AreEqual(transacao.Status.MensagemErro, mensagemErro);
        }

        [Test]
        public void AoReprocessarTransacaoDeveCriarNovaTransacaoComDadosCorretos()
        {
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Erro);
            var emissorId = Guid.NewGuid().ToString();
            var pagadorId = Guid.NewGuid().ToString();
            var valor = 223;

            var novaTransacao = transacao.Reprocessar(valor);
            Assert.AreEqual(valor, novaTransacao.Valor);
            Assert.AreEqual(transacao.CobrancaId, novaTransacao.CobrancaId);
            Assert.AreEqual(StatusTransacaoCartao.Procesando, novaTransacao.Status.Status);
        }
#endregion

#region EVENTS
        [Test]
        public void AoCriarTransacaoCartaoDeveAdicionarEventoTransacaoCartao()
        {
            var transacao = TransacaoCartaoBuilder.BuildTransacao();
            var transacaoCartaoEvent = transacao.Events
                .Where(e => e is TransacaoCartaoCriadaEvent)
                .Cast<TransacaoCartaoCriadaEvent>();

            Assert.AreEqual(transacaoCartaoEvent.SingleOrDefault()?.TransacaoId, transacao.Id.ToString());
        }

        [Test]
        public void AoAprovarTransacaoCartaoDeveAdicionarEventoTransacaoCartao()
        {
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Procesando);
            var transacaoCartaoEvents = transacao
                .Events
                .Where(e => e is TransacaoCartaoAprovadaEvent)
                .Cast<TransacaoCartaoAprovadaEvent>();

            Assert.IsNull(transacaoCartaoEvents.SingleOrDefault());

            transacao.Aprovar(Guid.NewGuid().ToString());

            Assert.AreSame(transacaoCartaoEvents.SingleOrDefault().Transacao, transacao);
        }

        [Test]
        public void AoDefinirTransacaoCartaoComoErroDeveAdicionarEventoTransacaoCartao()
        {
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Procesando);
            var transacaoCartaoEvents = transacao
                .Events
                .Where(e => e is ErroTransacaoCartaoEvent)
                .Cast<ErroTransacaoCartaoEvent>();

            Assert.IsNull(transacaoCartaoEvents.SingleOrDefault());

            transacao.DefinirErro("Sem limite", Guid.NewGuid().ToString());

            Assert.AreSame(transacaoCartaoEvents.SingleOrDefault().Transacao, transacao);
        }

        [Test]
        public void AoReprocessarTransacaoCartaoDeveAdicionarEventoATranscaoCartao()
        {
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Erro);
            var transacaoCartaoEvents = transacao
                .Events
                .Where(e => e is ReprocessandoTransacaoCartaoEvent)
                .Cast<ReprocessandoTransacaoCartaoEvent>();

            Assert.IsNull(transacaoCartaoEvents.SingleOrDefault());

            var novaTransacao = transacao.Reprocessar(22);
            var transacaoCartaoEventsNovaTransacao = novaTransacao
                .Events
                .Where(e => e is ReprocessandoTransacaoCartaoEvent)
                .Cast<ReprocessandoTransacaoCartaoEvent>();

            Assert.IsNull(transacaoCartaoEvents.SingleOrDefault());

            Assert.AreSame(transacaoCartaoEventsNovaTransacao.SingleOrDefault().Transacao, novaTransacao);
            Assert.AreSame(transacaoCartaoEventsNovaTransacao.SingleOrDefault().TransacaoAnterior, transacao);
        }

        #endregion

#region EXCECAO
        [Test]
        public void AoAprovarTransacaoComStatusDiferenteProcessandoDeveLancarExcecao([Values(
            StatusTransacaoCartao.Erro, StatusTransacaoCartao.Aprovada)] StatusTransacaoCartao statusAtual)
        {
            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(statusAtual);
            Assert.Throws<ImpossivelAprovarTransacaoException>(() => transacaoCartao.Aprovar(Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoDefinirErroTransacaoComStatusDiferenteProcessandoDeveLancarExcecao([Values(
            StatusTransacaoCartao.Erro, StatusTransacaoCartao.Aprovada)] StatusTransacaoCartao statusAtual)
        {
            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(statusAtual);
            Assert.Throws<ImpossivelDefinirErroTransacaoException>(() => transacaoCartao.DefinirErro("Falha", Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoReprocessarTransacaoComStatusDiferenteErroDeveLancarExcecao([Values(
                StatusTransacaoCartao.Procesando, StatusTransacaoCartao.Aprovada)]
            StatusTransacaoCartao statusAtual)
        {
            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(statusAtual);
            Assert.Throws<ImpossivelReprocessarTransacaoException>(() => transacaoCartao.Reprocessar(transacaoCartao.Valor));
        }
#endregion

    }

    public static class TransacaoCartaoBuilder
    {
        public static Transacao BuildTransacao() 
            => new Transacao(Guid.NewGuid().ToString(), Guid.NewGuid(), 200);

        public static Transacao BuildTransacao(string idCobranca, Guid idCartao, decimal valor)
            => new Transacao(idCobranca, idCartao, valor);

        public static Transacao ComStatus(this Transacao transacao, StatusTransacaoCartao status)
        {
            if (status == StatusTransacaoCartao.Erro)
            {
                return transacao.DefinirErro("Falha", Guid.NewGuid().ToString());
            }
            else if (status == StatusTransacaoCartao.Aprovada)
            {
                return transacao.Aprovar(Guid.NewGuid().ToString());
            }
            else if (status == StatusTransacaoCartao.Procesando)
                return BuildTransacao();

            return null;
        }

        public static Transacao ComStatusAprovado(this Transacao transacao) 
            => transacao.Aprovar(Guid.NewGuid().ToString());

        public static Transacao ComStatusErro(this Transacao transacao, string idTransacao, string mensagemErro) 
            => transacao.DefinirErro(mensagemErro, idTransacao);

        public static Transacao ComStatusAprovado(this Transacao transacao, string idTransacao)
            => transacao.Aprovar(idTransacao);
    }
}
