﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.ViewModels;
using Collectio.Domain.ClienteAggregate;

namespace Collectio.Application.Clientes.Commands
{
    public class CreateClienteCommandHandler : ICommandHandler<CreateClienteCommand, ClienteViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IClientesRepository _clienteRepository;

        public CreateClienteCommandHandler(IMapper mapper, IClientesRepository clienteRepository)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        public async Task<ClienteViewModel> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            var novoCliente = _mapper.Map<Cliente>(request);
            await _clienteRepository.SaveAsync(novoCliente);
            return _mapper.Map<ClienteViewModel>(novoCliente);
        }
    }
}