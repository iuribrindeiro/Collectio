using System;
using System.Threading.Tasks;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Commands.CommandsResponses;
using Collectio.Domain.Base;
using Collectio.Domain.ClienteAggregate;
using Microsoft.Extensions.Logging;

namespace Collectio.Application.Commands.Handlers
{
    public class CreateClienteCommandHandler : AbstractCommandHandler<CreateClienteCommand, CreateClienteCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IClientesRepository _clienteRepository;
        private readonly IRepository<Cliente> _clienteRepo2;

        public CreateClienteCommandHandler(IMapper mapper, IClientesRepository clienteRepository, IRepository<Cliente> clienteRepo2, ILogger<CreateClienteCommandHandler> logger) : base(logger)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
            _clienteRepo2 = clienteRepo2;
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
            await _clienteRepo2.UpdateAsync(atualizaCliente);

            _logger.LogInformation($"novo {novoCliente.Nome}; {novoCliente.Id}");
            _logger.LogInformation($"atualiza {atualizaCliente.Nome}; {atualizaCliente.Id}");

            return CreateClienteCommandResponse.Success(novoCliente);
        }
    }
}