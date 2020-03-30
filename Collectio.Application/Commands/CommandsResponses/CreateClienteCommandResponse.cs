using Collectio.Application.Base.Commands;
using Collectio.Domain.ClienteAggregate;

namespace Collectio.Application.Commands.CommandsResponses
{
    public class CreateClienteCommandResponse : CommandResponse
    {
        protected CreateClienteCommandResponse(Cliente cliente) 
            => Cliente = cliente;

        public static CreateClienteCommandResponse Success(Cliente cliente) 
            => new CreateClienteCommandResponse(cliente);

        public Cliente Cliente { get; set; }
    }
}