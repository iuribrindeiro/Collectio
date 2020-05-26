using System;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.Events;

namespace Collectio.Domain.CartaoCreditoAggregate.EventHandlers
{
    public class CobrancaCriacaEventHandler : IDomainEventHandler<CobrancaCriadaEvent>
    {
        private readonly ICobrancasRepository _cobrancasRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public CobrancaCriacaEventHandler(ICobrancasRepository cobrancasRepository, ICartaoCreditoRepository cartaoCreditoRepository)
        {
            _cobrancasRepository = cobrancasRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

        public async Task Handle(CobrancaCriadaEvent domainEvent, CancellationToken cancellationToken)
        {
            var cobranca = await _cobrancasRepository.FindAsync(Guid.Parse(domainEvent.CobrancaId));
            if (cobranca.FormaPagamentoCartao &&
                CartaoCreditoExiste(cobranca, out CartaoCredito cartaoCredito) && cartaoCredito.ProcessamentoFinalizado)
            {
                cartaoCredito.AddTransacao(cobranca.Id.ToString(), cobranca.ConfiguracaoEmissaoId, cobranca.Valor);
            }
        }

        private bool CartaoCreditoExiste(Cobranca cobranca, out CartaoCredito cartaoCredito)
        {
            cartaoCredito = null;
            return Guid.TryParse(cobranca.ClienteCobranca.CartaoCredito.TenantId, out Guid cartaoCreditoId) &&
                   _cartaoCreditoRepository.Exists(cartaoCreditoId, out cartaoCredito);
        }
    }
}
