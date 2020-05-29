using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Domain.CartaoCreditoAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace Collectio.Application.CartoesCredito.Commands
{
    public class CreateCartaoCreditoCommandHandler : ICommandHandler<CreateCartaoCreditoCommand, string>
    {
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly IMapper _mapper;

        public CreateCartaoCreditoCommandHandler(ICartaoCreditoRepository cartaoCreditoRepository, IMapper mapper)
        {
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateCartaoCreditoCommand request, CancellationToken cancellationToken)
        {
            var cartaoCredito = _mapper.Map<CartaoCredito>(request);
            await _cartaoCreditoRepository.SaveAsync(cartaoCredito);
            return cartaoCredito.Id.ToString();
        }
    }
}