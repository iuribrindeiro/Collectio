using System;
using Collectio.Application.Base.Commands;

namespace Collectio.Application.Commands
{
    public class UpdateClienteCommand : Command<CommandResponse>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}