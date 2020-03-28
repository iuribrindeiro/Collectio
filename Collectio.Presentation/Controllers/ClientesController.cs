using System.Threading.Tasks;
using Collectio.Application.Base;
using Collectio.Application.Commands;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Collectio.Presentation.Controllers
{
    public class ClientesController : BaseController
    {
        public ClientesController(ICommandQuerySender commandQuerySender) : base(commandQuerySender)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateClienteCommand createClienteCommand) 
            => await Send(createClienteCommand);
    }
}
