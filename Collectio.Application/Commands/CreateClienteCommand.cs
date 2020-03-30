using System;
using Collectio.Application.Base.Commands;
using Collectio.Application.Commands.CommandsResponses;

namespace Collectio.Application.Commands
{
    public class CreateClienteCommand : Command<CreateClienteCommandResponse>
    {
        public string Nome { get; set; }
    }
}