using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Domain.ClienteAggregate;
using Collectio.Infra.CrossCutting.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Collectio.Application.ViewModels;

namespace Collectio.Application.Commands.Handlers
{
    public class CreateClienteCommandHandler : AbstractCommandHandler<CreateClienteCommand, CommandResponseData<ClienteViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IClientesRepository _clienteRepository;

        public CreateClienteCommandHandler(IMapper mapper, IClientesRepository clienteRepository, 
            ILogger<CreateClienteCommandHandler> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        protected override async Task<CommandResponseData<ClienteViewModel>> HandleAsync(CreateClienteCommand command)
        {
            var novoCliente = _mapper.Map<Cliente>(command);
            await _clienteRepository.SaveAsync(novoCliente);
            
            return CommandResponseData<ClienteViewModel>.Success(_mapper.Map<ClienteViewModel>(novoCliente));
        }
    }
}