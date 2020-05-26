using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Cobrancas.Commands;
using Collectio.Domain.CobrancaAggregate;

namespace Collectio.Application.Cobrancas.CommandHandlers
{
    public class CreateCobrancaCartaoCommandHandler : ICommandHandler<CreateCobrancaCartaoCommand, string>
    {
        private readonly ICobrancasRepository _cobrancasRepository;
        private readonly IMapper _mapper;

        public CreateCobrancaCartaoCommandHandler(ICobrancasRepository cobrancasRepository, IMapper mapper)
        {
            _cobrancasRepository = cobrancasRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateCobrancaCartaoCommand request, CancellationToken cancellationToken)
        {
            var cobranca = _mapper.Map<Cobranca>(request);
            await _cobrancasRepository.SaveAsync(cobranca);
            return cobranca.Id.ToString();
        }
    }
}