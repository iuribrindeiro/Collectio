using Collectio.Domain.CartaoCreditoAggregate.Events;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.EventHandlers;
using Collectio.Domain.CobrancaAggregate.Events;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Collectio.Domain.Test
{
    public class TransacaoCartaoAprovadaEventHandlerTest
    {
        private ICobrancasRepository _cobrancasRepository;
        private TransacaoCartaoAprovadaEventHandler _sut;

        [SetUp]
        public void Setup()
        {
            _cobrancasRepository = Substitute.For<ICobrancasRepository>();
            _sut = new TransacaoCartaoAprovadaEventHandler(_cobrancasRepository);
        }

        [Test]
        public async Task AoBuscarCobrancaDaTransacaoDeveFinalizarCobranca()
        {
            var transacaoId = Guid.NewGuid().ToString();
            var cobrancaId = Guid.NewGuid();
            var cobranca = Cobranca.Cartao(1, DateTime.Today, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            _cobrancasRepository.FindAsync(cobrancaId).Returns(async c => cobranca);
            _sut.Handle(new TransacaoCartaoAprovadaEvent(transacaoId, cobrancaId.ToString()), CancellationToken.None).GetAwaiter().GetResult();
            Assert.IsNotNull(cobranca.Events.SingleOrDefault(e => e is FormaPagamentoProcessadaEvent));
        }
    }
}
