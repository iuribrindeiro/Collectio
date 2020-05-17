using System;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base;
using Collectio.Domain.CartaoCreditoAggregate.Events;

namespace Collectio.Domain.CobrancaAggregate.EventHandlers
{
    public class ErroTransacaoCartaoEventHandler : IDomainEventHandler<ErroTransacaoCartaoEvent>
    {
        private readonly ICobrancasRepository _cobrancasRepository;

        public ErroTransacaoCartaoEventHandler(ICobrancasRepository cobrancasRepository) 
            => _cobrancasRepository = cobrancasRepository;

        public async Task Handle(ErroTransacaoCartaoEvent domainEvent, CancellationToken cancellationToken)
        {
            var cobranca = await _cobrancasRepository.FindAsync(Guid.Parse(domainEvent.TransacaoId));
            cobranca.ErroCriarTransacao();
        }
    }
}