using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Commands.CommandsResponses;
using Collectio.Domain.ClienteAggregate;
using Collectio.Infra.CrossCutting.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Collectio.Application.Commands.Handlers
{
    public class CreateClienteCommandHandler : AbstractCommandHandler<CreateClienteCommand, CreateClienteCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IClientesRepository _clienteRepository;

        public CreateClienteCommandHandler(IMapper mapper, IClientesRepository clienteRepository, 
            ILogger<CreateClienteCommandHandler> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        protected override async Task<CreateClienteCommandResponse> HandleAsync(CreateClienteCommand command)
        {
            var updateCommand = new UpdateClienteCommand()
            {
                Id = Guid.NewGuid(),
                Nome = "Teste 2"
            };

            var novoCliente = _mapper.Map<Cliente>(command);
            var atualizaCliente = _mapper.Map<Cliente>(updateCommand);

            await _clienteRepository.SaveAsync(novoCliente);

            _logger.LogInformation($"novo {novoCliente.Nome}; {novoCliente.Id}");
            _logger.LogInformation($"atualiza {atualizaCliente.Nome}; {atualizaCliente.Id}");

            return CreateClienteCommandResponse.Success(novoCliente);
        }
    }
}