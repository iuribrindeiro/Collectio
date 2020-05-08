using Collectio.Application.Base;
using Collectio.Application.Commands;
using Collectio.Application.Queries.Handlers;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Collectio.Application.Queries;
using Microsoft.AspNetCore.Authorization;

namespace Collectio.Presentation.Controllers
{
    [Authorize]
    public class ClientesController : BaseController
    {
        public ClientesController(ICommandQuerySender commandQuerySender) : base(commandQuerySender) {}

        [HttpPost]
        public async Task<IActionResult> Post(CreateClienteCommand createClienteCommand) 
            => await Send(createClienteCommand);

        [HttpGet, EnableQuery]
        public async Task<IActionResult> Get([FromQuery]ClienteQueryRequest query)
            => await Send(query);
    }
}
