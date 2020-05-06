using Collectio.Application.Base.Commands;
using Collectio.Application.ViewModels;
using MediatR;

namespace Collectio.Application.Commands
{
    public class CreateClienteCommand : ICommand<ClienteViewModel>
    {
        public string Nome { get; set; }
    }
}