using System;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.ViewModels;
using Collectio.Domain.ClienteAggregate;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.ContaBancarias;
using Collectio.Domain.CobrancaAggregate.Entidades;
using MediatR;

namespace Collectio.Application.Commands.Handlers
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
            var cpfCnpjValueObject = new CpfCnpjValueObject("61315286327");
            var endereco = new Endereco("rua", "1234", "bauirro", "60810820", "cidade", "CE");
            var cobranca = new Cobranca(1, DateTime.Today,
                new Pagador("TEste", cpfCnpjValueObject, endereco),
                new Emissor("Iuri", cpfCnpjValueObject, endereco),
                new ContaBancaria("Conta do Banco do Brasil Fake", "BB", new AgenciaContaValueObject("12345", "1234")),
                FormaPagamento.Boleto);

            var lol = cobranca.Errors;
            var novoCliente = _mapper.Map<Cliente>(request);
            novoCliente = null;
            await _clienteRepository.SaveAsync(novoCliente);
            return _mapper.Map<ClienteViewModel>(novoCliente);
        }
    }
}