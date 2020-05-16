using System;
using System.Linq;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.TransacaoCartaoAggregate;
using Collectio.Domain.TransacaoCartaoAggregate.Events;
using Collectio.Domain.TransacaoCartaoAggregate.Exceptions;
using NUnit.Framework;

namespace Collectio.Domain.Test
{
    public class TransacaoCartaoTest
    {

#region SETAR DADOS CORRETAMENTE
        [Test]
        public void AoCriarTransacaoCartaoTodosOsCamposDeveSerSetadosCorretamente()
        {
            var idCobranca = Guid.NewGuid().ToString();
            var emissorId = Guid.NewGuid().ToString();
            var pagadorId = Guid.NewGuid().ToString();
            var valor = 200;
            var vencimentoCartao = DateTime.Today.AddDays(5);
            var numero = "123456789";
            var codigoSeguranca = "123";
            var nome = "Teste Cartao bla";

            var cartaoValueObject = TransacaoCartaoBuilder.BuildDadosCartao(vencimentoCartao, numero, codigoSeguranca, nome);

            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao(idCobranca, emissorId, pagadorId, valor, cartaoValueObject);

            Assert.AreEqual(transacaoCartao.IdCobranca, idCobranca);
            Assert.AreEqual(transacaoCartao.EmissorId, emissorId);
            Assert.AreEqual(transacaoCartao.PagadorId, pagadorId);
            Assert.AreEqual(transacaoCartao.Valor, valor);
            Assert.AreSame(transacaoCartao.Cartao, cartaoValueObject);

            Assert.AreEqual(vencimentoCartao, transacaoCartao.Cartao.Vencimento);
            Assert.AreEqual(cartaoValueObject.Numero, transacaoCartao.Cartao.Numero);
            Assert.AreEqual(codigoSeguranca, transacaoCartao.Cartao.CodigoSeguranca);
            Assert.AreEqual(nome, transacaoCartao.Cartao.Nome);
        }

        [Test]
        public void AoAprovarTransacaoDeveSetarOIdTransacaoCorretamente()
        {
            var idTransacao = Guid.NewGuid().ToString();
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Procesando);
            transacao.Aprovar(idTransacao);
            Assert.AreEqual(transacao.Status.TransacaoId, idTransacao);
        }

        [Test]
        public void AoDefinirTransacaoComoErroDeveSetarIdTransacaoEMensagemCorretamente()
        {
            var mensagemErro = "Sem limite";
            var idTransacao = Guid.NewGuid().ToString();
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Procesando);
            transacao.DefinirErro(mensagemErro, idTransacao);

            Assert.AreEqual(transacao.Status.TransacaoId, idTransacao);
            Assert.AreEqual(transacao.Status.MensagemErro, mensagemErro);
        }

        [Test]
        public void AoIniciarProcessamentoTranscaoDeveSetarTokenCartaoCorretamente()
        {
            var tokenCartao = Guid.NewGuid().ToString();
            var transacao = TransacaoCartaoBuilder.BuildTransacao();
            transacao.Processando(tokenCartao);

            Assert.AreEqual(transacao.Status.TokenCartao, tokenCartao);
        }

        [Test]
        public void AoReprocessarTransacaoDeveCriarNovaTransacaoComDadosCorretos()
        {
            var transacao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(StatusTransacaoCartao.Erro);
            var emissorId = Guid.NewGuid().ToString();
            var pagadorId = Guid.NewGuid().ToString();
            var valor = 223;

            var novaTransacao = transacao.Reprocessar(emissorId, pagadorId, valor);
            Assert.AreEqual(emissorId, novaTransacao.EmissorId);
            Assert.AreEqual(pagadorId, novaTransacao.PagadorId);
            Assert.AreEqual(valor, novaTransacao.Valor);
            Assert.AreEqual(transacao.IdCobranca, novaTransacao.IdCobranca);
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

            Assert.AreSame(transacaoCartaoEvent.SingleOrDefault()?.TransacaoCartao, transacao);
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

            Assert.AreSame(transacaoCartaoEvents.SingleOrDefault().TransacaoCartao, transacao);
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

            Assert.AreSame(transacaoCartaoEvents.SingleOrDefault().TransacaoCartao, transacao);
        }

        [Test]
        public void AoIniciarProcessamentoTransacaoDeveAdicionarEventoTransacaoCartao()
        {
            var transacao = TransacaoCartaoBuilder.BuildTransacao();
            var transacaoCartaoEvents = transacao
                .Events
                .Where(e => e is ProcessandoTransacaoCartaoEvent)
                .Cast<ProcessandoTransacaoCartaoEvent>();

            Assert.IsNull(transacaoCartaoEvents.SingleOrDefault());

            transacao.Processando(Guid.NewGuid().ToString());

            Assert.AreSame(transacaoCartaoEvents.SingleOrDefault().TransacaoCartao, transacao);
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

            var novaTransacao = transacao.Reprocessar(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 22);
            var transacaoCartaoEventsNovaTransacao = novaTransacao
                .Events
                .Where(e => e is ReprocessandoTransacaoCartaoEvent)
                .Cast<ReprocessandoTransacaoCartaoEvent>();

            Assert.IsNull(transacaoCartaoEvents.SingleOrDefault());

            Assert.AreSame(transacaoCartaoEventsNovaTransacao.SingleOrDefault().TransacaoCartao, novaTransacao);
            Assert.AreSame(transacaoCartaoEventsNovaTransacao.SingleOrDefault().TransacaoCartaoAnterior, transacao);
        }

        #endregion

#region EXCECAO
        [Test]
        public void AoIniciarProcessamentoComStatusDiferenteCriandoTokenCartaoDeveLancarExcecao([Values(
            StatusTransacaoCartao.Procesando, StatusTransacaoCartao.Erro, StatusTransacaoCartao.Aprovada)] StatusTransacaoCartao statusAtual)
        {
            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(statusAtual);
            Assert.Throws<ImpossivelIniciarProcessamentoTransacaoException>(() => transacaoCartao.Processando(Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoAprovarTransacaoComStatusDiferenteProcessandoDeveLancarExcecao([Values(
            StatusTransacaoCartao.Erro, StatusTransacaoCartao.Aprovada, StatusTransacaoCartao.CriandoTokenCartao)] StatusTransacaoCartao statusAtual)
        {
            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(statusAtual);
            Assert.Throws<ImpossivelAprovarTransacaoException>(() => transacaoCartao.Aprovar(Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoDefinirErroTransacaoComStatusDiferenteProcessandoDeveLancarExcecao([Values(
            StatusTransacaoCartao.Erro, StatusTransacaoCartao.Aprovada, StatusTransacaoCartao.CriandoTokenCartao)] StatusTransacaoCartao statusAtual)
        {
            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(statusAtual);
            Assert.Throws<ImpossivelDefinirErroTransacaoException>(() => transacaoCartao.DefinirErro("Falha", Guid.NewGuid().ToString()));
        }

        [Test]
        public void AoReprocessarTransacaoComStatusDiferenteErroDeveLancarExcecao([Values(
                StatusTransacaoCartao.Procesando, StatusTransacaoCartao.Aprovada,
                StatusTransacaoCartao.CriandoTokenCartao)]
            StatusTransacaoCartao statusAtual)
        {
            var transacaoCartao = TransacaoCartaoBuilder.BuildTransacao().ComStatus(statusAtual);
            Assert.Throws<ImpossivelReprocessarTransacaoException>(() => transacaoCartao.Reprocessar(transacaoCartao.EmissorId, transacaoCartao.PagadorId, transacaoCartao.Valor));
        }
#endregion

    }

    public static class TransacaoCartaoBuilder
    {
        public static TransacaoCartao BuildTransacao() 
            => new TransacaoCartao(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 200, BuildDadosCartao());

        public static TransacaoCartao BuildTransacao(CartaoValueObject cartao)
            => new TransacaoCartao(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 200, cartao);

        public static TransacaoCartao BuildTransacao(string idCobranca, string emissorId, string pagadorId, decimal valor, CartaoValueObject cartao)
            => new TransacaoCartao(idCobranca, emissorId, pagadorId, valor, cartao);

        public static CartaoValueObject BuildDadosCartao()
            => new CartaoValueObject(DateTime.Today, "1234", "123", "bla", new CpfCnpjValueObject("12345678912"));

        public static CartaoValueObject BuildDadosCartao(DateTime vencimento, string numero, string codigo, string nome, CpfCnpjValueObject cpfCnpjValueObject)
            => new CartaoValueObject(vencimento, numero, codigo, nome, cpfCnpjValueObject);

        public static CartaoValueObject BuildDadosCartao(DateTime vencimento, string numero, string codigo, string nome)
            => new CartaoValueObject(vencimento, numero, codigo, nome, new CpfCnpjValueObject("123456789"));

        public static TransacaoCartao ComStatus(this TransacaoCartao transacaoCartao, StatusTransacaoCartao status)
        {
            if (status == StatusTransacaoCartao.Erro)
            {
                if (transacaoCartao.Status.Status == StatusTransacaoCartao.CriandoTokenCartao)
                    transacaoCartao.Processando(Guid.NewGuid().ToString());

                return transacaoCartao.DefinirErro("Falha", Guid.NewGuid().ToString());
            } 
            else if (status == StatusTransacaoCartao.Procesando)
                return transacaoCartao.Processando(Guid.NewGuid().ToString());
            else if (status == StatusTransacaoCartao.Aprovada)
            {
                if (transacaoCartao.Status.Status == StatusTransacaoCartao.CriandoTokenCartao)
                    transacaoCartao.Processando(Guid.NewGuid().ToString());

                return transacaoCartao.Aprovar(Guid.NewGuid().ToString());
            }
                
            else if (status == StatusTransacaoCartao.CriandoTokenCartao)
                return BuildTransacao();

            return null;
        }

        public static TransacaoCartao ComStatusAprovado(this TransacaoCartao transacaoCartao) 
            => transacaoCartao.Aprovar(Guid.NewGuid().ToString());

        public static TransacaoCartao ComStatusProcessando(this TransacaoCartao transacaoCartao)
            => transacaoCartao.Processando(Guid.NewGuid().ToString());

        public static TransacaoCartao ComStatusErro(this TransacaoCartao transacaoCartao, string idTransacao, string mensagemErro)
        {
            if (transacaoCartao.Status.Status == StatusTransacaoCartao.CriandoTokenCartao)
                transacaoCartao.Processando(Guid.NewGuid().ToString());

            return transacaoCartao.DefinirErro(mensagemErro, idTransacao);
        }

        public static TransacaoCartao ComStatusAprovado(this TransacaoCartao transacaoCartao, string idTransacao)
            => transacaoCartao.Aprovar(idTransacao);
    }
}
