using Collectio.Domain.Base;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.CartaoCreditoAggregate;

namespace Collectio.Domain.ClienteAggregate.EventHandlers
{
    public class CartaoCreditoCriadoEventHandler : IDomainEventHandler<CartaoCreditoCriadoEvent>
    {
        private readonly IClientesRepository _clientesRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public CartaoCreditoCriadoEventHandler(IClientesRepository clientesRepository, ICartaoCreditoRepository cartaoCreditoRepository)
        {
            _clientesRepository = clientesRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

        public async Task Handle(CartaoCreditoCriadoEvent notification, CancellationToken cancellationToken)
        {
            var cartaoCredito = await _cartaoCreditoRepository.FindAsync(Guid.Parse(notification.CartaoId));
            var cliente = await _clientesRepository.FindByCpfCnpjAsync(cartaoCredito.CpfCnpjProprietario);
            if (cliente && !cliente.CartaoCreditoPadraoDefinido)
                cliente.DefinirCartaoCreditoPadrao(cartaoCredito.Id.ToString());
        }
    }
}
