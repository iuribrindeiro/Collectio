using System;
using Collectio.Application.Base.Commands;
using Collectio.Application.ViewModels;

namespace Collectio.Application.Commands
{
    public class UpdateClienteCommand : ICommand<ClienteViewModel>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}