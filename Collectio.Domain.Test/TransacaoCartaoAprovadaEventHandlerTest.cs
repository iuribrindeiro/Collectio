using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.EventHandlers;
using Collectio.Domain.CobrancaAggregate.Events;
using Collectio.Domain.CobrancaAggregate.Exceptions;
using NSubstitute;
using NUnit.Framework;

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
        public void AoBuscarCobrancaDaTransacaoDeveFinalizarCobranca()
        {
            var transacaoId = Guid.NewGuid().ToString();
            var cobrancaId = Guid.NewGuid();
            var cobranca = Cobranca.Boleto(1, DateTime.Today, Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            _cobrancasRepository.FindAsync(cobrancaId).Returns(async c => cobranca);
            _sut.Handle(new TransacaoCartaoAprovadaEvent(transacaoId, cobrancaId.ToString()), CancellationToken.None).GetAwaiter().GetResult();
            Assert.IsNotNull(cobranca.Events.SingleOrDefault(e => e is FormaPagamentoProcessadaEvent));
        }
    }
}
