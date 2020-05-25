using Collectio.Application.Clientes.Commands;
using Collectio.Application.Queries;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Collectio.Presentation.Controllers
{
    [Authorize]
    public class ClientesController : BaseController
    {
        [HttpPost]
        public Task<IActionResult> Post(CreateClienteCommand createClienteCommand) 
            => Send(createClienteCommand);

        [HttpGet, EnableQuery]
        public Task<IActionResult> Get([FromQuery]ClienteQueryRequest query)
            => Send(query);
    }
}
