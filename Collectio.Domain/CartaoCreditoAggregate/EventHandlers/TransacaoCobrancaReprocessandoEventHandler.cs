using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.Events;

namespace Collectio.Domain.CartaoCreditoAggregate.EventHandlers
{
    public class TransacaoCobrancaReprocessandoEventHandler : IDomainEventHandler<TransacaoCobrancaReprocessandodoEvent>
    {
        private readonly ICobrancasRepository _cobrancasRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public TransacaoCobrancaReprocessandoEventHandler(ICobrancasRepository cobrancasRepository, ICartaoCreditoRepository cartaoCreditoRepository)
        {
            _cobrancasRepository = cobrancasRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

        public async Task Handle(TransacaoCobrancaReprocessandodoEvent domainEvent, CancellationToken cancellationToken)
        {
            var cobranca = await _cobrancasRepository.FindAsync(Guid.Parse(domainEvent.CobrancaId));
            var transacao = _cartaoCreditoRepository
                .ListaTransacoesCobranca(domainEvent.CobrancaId)
                .OrderByDescending(t => t.DataCriacao)
                .FirstOrDefault();

            transacao.Reprocessar(cobranca.Valor, cobranca.ContaBancariaId);
        }
    }
}