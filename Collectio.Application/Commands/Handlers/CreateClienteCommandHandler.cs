using System;
using System.Threading.Tasks;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Domain.ClienteAggregate;
using Microsoft.Extensions.Logging;

namespace Collectio.Application.Commands.Handlers
{
    public class CreateClienteCommandHandler : AbstractCommandHandler<CreateClienteCommand, CreateClienteCommandResponse>
    {
        private readonly IMapper _mapper;

        public CreateClienteCommandHandler(IMapper mapper, ILogger<CreateClienteCommandHandler> logger) : base(logger) 
            => _mapper = mapper;

        protected override async Task<CreateClienteCommandResponse> HandleAsync(CreateClienteCommand command)
        {
            var updateCommand = new UpdateClienteCommand()
            {
                Id = Guid.NewGuid(),
                Nome = "Teste 2"
            };

            var novoCliente = _mapper.Map<Cliente>(command);
            var atualizaCliente = _mapper.Map<Cliente>(updateCommand);

            _logger.LogInformation($"novo {novoCliente.Nome}; {novoCliente.Id}");
            _logger.LogInformation($"atualiza {atualizaCliente.Nome}; {atualizaCliente.Id}");

            return CreateClienteCommandResponse.Success(novoCliente);
        }
    }
}