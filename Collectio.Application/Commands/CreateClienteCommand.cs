using System;
using Collectio.Application.Base.Commands;
using Collectio.Domain.ClienteAggregate;

namespace Collectio.Application.Commands
{
    public class CreateClienteCommand : Command<CreateClienteCommandResponse>
    {
        public string Nome { get; set; }
    }

    public class CreateClienteCommandResponse : CommandResponse
    {
        protected CreateClienteCommandResponse(Cliente cliente) 
            => Cliente = cliente;

        public static CreateClienteCommandResponse Success(Cliente cliente) 
            => new CreateClienteCommandResponse(cliente);

        public Cliente Cliente { get; set; }
    }
}