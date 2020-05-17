using Collectio.Domain.Base;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Collectio.Domain.CobrancaAggregate.EventHandlers
{
    public class TransacaoCartaoAprovadaEventHandler : IDomainEventHandler<TransacaoCartaoAprovadaEvent>
    {
        private readonly ICobrancasRepository _cobrancasRepository;

        public TransacaoCartaoAprovadaEventHandler(ICobrancasRepository cobrancasRepository) 
            => _cobrancasRepository = cobrancasRepository;

        public async Task Handle(TransacaoCartaoAprovadaEvent domainEvent, CancellationToken cancellationToken)
        {
            var cobranca = await _cobrancasRepository.FindAsync(Guid.Parse(domainEvent.CobrancaId));
            cobranca.FinalizaProcessamentoFormaPagamento();
        }
    }
}
