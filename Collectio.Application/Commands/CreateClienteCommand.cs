using Collectio.Application.Base.Commands;
using Collectio.Application.ViewModels;

namespace Collectio.Application.Commands
{
    public class CreateClienteCommand : Command<CommandResponseData<ClienteViewModel>>
    {
        public string Nome { get; set; }
    }
}